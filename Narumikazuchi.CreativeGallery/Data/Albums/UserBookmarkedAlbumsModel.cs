using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Albums;

[PrimaryKey(nameof(UserIdentifier), nameof(AlbumIdentifier))]
public sealed record class UserBookmarkedAlbumsModel
{
    public required Guid UserIdentifier
    {
        get;
        set;
    }

    public required Guid AlbumIdentifier
    {
        get;
        set;
    }
}