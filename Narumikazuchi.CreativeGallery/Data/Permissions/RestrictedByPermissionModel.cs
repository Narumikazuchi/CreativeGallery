using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Permissions;

[PrimaryKey(nameof(RestrictionIdentifier), nameof(PermissionIdentifier))]
public sealed record class RestrictedByPermissionModel
{
    public required Guid RestrictionIdentifier
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