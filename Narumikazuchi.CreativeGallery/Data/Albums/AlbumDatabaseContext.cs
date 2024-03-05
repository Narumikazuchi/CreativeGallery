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

    public async Task<Optional<AlbumModel>> LoadAlbumAsynchronously(Guid identifier,
                                                                    CancellationToken cancellationToken = default)
    {
        Optional<AlbumModel> result = await this.Context.Albums.Include(entity => entity.Works)
                                                               .Include(entity => entity.LikedByUsers)
                                                               .Include(entity => entity.BookmarkedByUsers)
                                                               .Include(entity => entity.Owner)
                                                               .FirstOrDefaultAsync(entity => entity.Identifier == identifier,
                                                                                    cancellationToken: cancellationToken);
        return result;
    }
}