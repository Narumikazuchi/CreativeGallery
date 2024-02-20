using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Users;

[PrimaryKey(nameof(UserIdentifier), nameof(CreatorIdentifier))]
public sealed record class UserFollowsCreatorModel
{
    public required Guid UserIdentifier
    {
        get;
        set;
    }

    public required Guid CreatorIdentifier
    {
        get;
        set;
    }
}