using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Albums;

[PrimaryKey(nameof(AlbumIdentifier), nameof(CreativeWorkIdentifier))]
public sealed record class CreativeWorkInAlbumModel
{
    public required Guid AlbumIdentifier
    {
        get;
        set;
    }

    public required Guid CreativeWorkIdentifier
    {
        get;
        set;
    }
}