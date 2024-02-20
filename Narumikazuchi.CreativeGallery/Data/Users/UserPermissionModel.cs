using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Users;

[PrimaryKey(nameof(UserIdentifier), nameof(PermissionIdentifier))]
public sealed record class UserPermissionModel
{
    public required Guid UserIdentifier
    {
        get;
        set;
    }

    public required Guid PermissionIdentifier
    {
        get;
        set;
    }
}