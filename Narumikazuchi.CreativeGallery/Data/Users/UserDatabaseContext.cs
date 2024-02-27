using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Permissions;
using Narumikazuchi.CreativeGallery.Data.Search;

namespace Narumikazuchi.CreativeGallery.Data.Users;

public sealed class UserDatabaseContext : DatabaseContext
{
    public UserDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public async ValueTask AddUserAsynchronously(UserModel user,
                                                 AuthenticationModel authentication,
                                                 CancellationToken cancellationToken = default)
    {
        if (user.ProfilePicture is null)
        {
            await using FileStream stream = File.OpenRead(path: DEFAULT_PROFILE_FILENAME);
            Byte[] bytes = new Byte[stream.Length];
            _= await stream.ReadAsync(buffer: bytes,
                                      cancellationToken: cancellationToken);
            user.ProfilePicture = bytes;
        }

        _ = await this.Context.Users.AddAsync(entity: user,
                                              cancellationToken: cancellationToken);
        _ = await this.Context.Authentications.AddAsync(entity: authentication,
                                                        cancellationToken: cancellationToken);

        if (user.Visibility is DataVisibility.Public)
        {
            SearchResultModel searchResult = new()
            {
                Identifier = user.Identifier,
                Count = 0,
                Value = user.DisplayName,
                Type = SearchResultType.User
            };
            _ = await this.Context.SearchQuery.AddAsync(entity: searchResult,
                                                        cancellationToken: cancellationToken);
        }
    }

    public async ValueTask AddAuthenticationAsynchronously(AuthenticationModel authentication,
                                                           CancellationToken cancellationToken = default)
    {
        _ = await this.Context.Authentications.AddAsync(entity: authentication,
                                                        cancellationToken: cancellationToken);
    }

    public async ValueTask AddPermissionAsynchronously(PermissionModel permission,
                                                       CancellationToken cancellationToken = default)
    {
        _ = await this.Context.Permissions.AddAsync(entity: permission,
                                                    cancellationToken: cancellationToken);
    }

    public async Task<Boolean> AuthenticateUserAsynchronously(UserModel user,
                                                              AuthenticationType type,
                                                              Byte[] key,
                                                              CancellationToken cancellationToken = default)
    {
        Optional<AuthenticationModel> authentication;
        if (user.Authentications.Count is not 0)
        {
            authentication = user.Authentications.FirstOrDefault(entity => entity.Type == type);
        }
        else
        {
            authentication = await this.Context.Authentications.FirstOrDefaultAsync(entity => entity.UserIdentifier == user.Identifier &&
                                                                                              entity.Type == type,
                                                                                    cancellationToken: cancellationToken);
        }

        if (authentication.HasValue is false)
        {
            return false;
        }

        return key.SequenceEqual(second: authentication.Value.StoredKey) is true;
    }

    public async Task<Optional<UserModel>> FindUserAsynchronously(Expression<Func<UserModel, Boolean>> predicate,
                                                                  CancellationToken cancellationToken = default)
    {
        Optional<UserModel> result = await this.Context.Users.Include(entity => entity.Authentications)
                                                             .Include(entity => entity.FollowedByUsers)
                                                             .FirstOrDefaultAsync(predicate: predicate,
                                                                                  cancellationToken: cancellationToken);
        return result;
    }

    public async Task<Optional<PermissionModel>> FindPermissionAsynchronously(Expression<Func<PermissionModel, Boolean>> predicate,
                                                                              CancellationToken cancellationToken = default)
    {
        Optional<PermissionModel> result = await this.Context.Permissions.FirstOrDefaultAsync(predicate: predicate,
                                                                                              cancellationToken: cancellationToken);
        return result;
    }

    public async ValueTask MakeUserFollowCreatorAsynchronously(UserModel user,
                                                               UserModel creator,
                                                               CancellationToken cancellationToken = default)
    {
        Optional<UserModel> userWithFollowed = await this.Context.Users.Include(entity => entity.FollowedByUsers)
                                                                       .FirstOrDefaultAsync(entity => entity.Identifier == creator.Identifier,
                                                                                            cancellationToken: cancellationToken);

        Optional<UserModel> userWithFollows = await this.Context.Users.Include(entity => entity.FollowsUsers)
                                                                      .FirstOrDefaultAsync(entity => entity.Identifier == user.Identifier,
                                                                                           cancellationToken: cancellationToken);
        if (userWithFollows.HasValue is false ||
            userWithFollowed.HasValue is false)
        {
            return;
        }

        user = userWithFollows.Value;
        creator = userWithFollowed.Value;

        user.FollowsUsers.Add(item: creator);
        creator.FollowedByUsers.Add(item: user);

        if (creator.Visibility is not DataVisibility.Public)
        {
            return;
        }

        Optional<SearchResultModel> searchResult = await this.Context.SearchQuery.FirstOrDefaultAsync(entity => entity.Identifier == creator.Identifier,
                                                                                                      cancellationToken: cancellationToken);
        if (searchResult.HasValue is true)
        {
            searchResult.Value.Count = (UInt32)creator.FollowedByUsers.Count;
        }
        else
        {
            SearchResultModel result = new()
            {
                Identifier = creator.Identifier,
                Value = creator.DisplayName,
                Type = SearchResultType.User,
                Count = (UInt32)creator.FollowedByUsers.Count,
            };
            _ = await this.Context.SearchQuery.AddAsync(entity: result,
                                                        cancellationToken: cancellationToken);
        }
    }

    private const String DEFAULT_PROFILE_FILENAME = "DefaultProfile.png";
}