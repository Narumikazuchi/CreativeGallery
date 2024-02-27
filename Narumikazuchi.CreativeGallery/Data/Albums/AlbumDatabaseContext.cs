using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Search;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data.Albums;

public sealed class AlbumDatabaseContext : DatabaseContext
{
    public AlbumDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public async ValueTask MakeUserLikeAlbumAsynchronously(UserModel user,
                                                           AlbumModel album,
                                                           CancellationToken cancellationToken = default)
    {
        Optional<AlbumModel> albumWithLiked = await this.Context.Albums.Include(entity => entity.LikedByUsers)
                                                                       .FirstOrDefaultAsync(entity => entity.Identifier == album.Identifier,
                                                                                            cancellationToken: cancellationToken);

        Optional<UserModel> userWithLikes = await this.Context.Users.Include(entity => entity.LikedAlbums)
                                                                    .FirstOrDefaultAsync(entity => entity.Identifier == user.Identifier,
                                                                                         cancellationToken: cancellationToken);
        if (albumWithLiked.HasValue is false ||
            userWithLikes.HasValue is false)
        {
            return;
        }

        user = userWithLikes.Value;
        album = albumWithLiked.Value;

        user.LikedAlbums.Add(item: album);
        album.LikedByUsers.Add(item: user);

        if (album.Visibility is not DataVisibility.Public)
        {
            return;
        }

        Optional<SearchResultModel> searchResult = await this.Context.SearchQuery.FirstOrDefaultAsync(entity => entity.Identifier == album.Identifier,
                                                                                                      cancellationToken: cancellationToken);
        if (searchResult.HasValue is true)
        {
            searchResult.Value.Count = (UInt32)album.LikedByUsers.Count;
        }
        else
        {
            searchResult = new SearchResultModel()
            {
                Identifier = album.Identifier,
                Value = album.Name,
                Type = SearchResultType.Album,
                Count = (UInt32)album.LikedByUsers.Count,
            };
            _ = await this.Context.SearchQuery.AddAsync(entity: searchResult.Value!,
                                                        cancellationToken: cancellationToken);
        }
    }
}