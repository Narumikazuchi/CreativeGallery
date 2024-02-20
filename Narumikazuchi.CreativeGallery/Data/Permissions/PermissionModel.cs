using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data.Permissions;

[PrimaryKey(nameof(Identifier))]
public sealed record class PermissionModel
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

    public Boolean CanBeDeleted
    {
        get;
        init;
    } = true;

    public List<UserModel> Users
    {
        get;
        init;
    } = [];

    public List<RestrictionModel> Allows
    {
        get;
        init;
    } = [];
}