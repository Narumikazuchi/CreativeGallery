using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Permissions;

[PrimaryKey(nameof(Identifier))]
public sealed record class RestrictionModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required String Name
    {
        get;
        init;
    }

    public List<PermissionModel> AllowedFor
    {
        get;
        init;
    } = [];
}