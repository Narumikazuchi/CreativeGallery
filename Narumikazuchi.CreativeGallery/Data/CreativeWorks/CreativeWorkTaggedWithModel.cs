using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.CreativeWorks;

[PrimaryKey(nameof(CreativeWorkIdentifier), nameof(TagIdentifier))]
public sealed record class CreativeWorkTaggedWithModel
{
    public required Guid CreativeWorkIdentifier
    {
        get;
        set;
    }

    public required Guid TagIdentifier
    {
        get;
        set;
    }
}