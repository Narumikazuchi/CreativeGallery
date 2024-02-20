using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.CreativeWorks;

[PrimaryKey(nameof(UserIdentifier), nameof(CreativeWorkIdentifier))]
public sealed record class UserLikedWorksModel
{
    public required Guid UserIdentifier
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