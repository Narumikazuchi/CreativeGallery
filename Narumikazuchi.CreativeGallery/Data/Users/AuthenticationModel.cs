using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Users;

[PrimaryKey(nameof(Identifier))]
public sealed record class AuthenticationModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required Guid UserIdentifier
    {
        get;
        init;
    }

    public required UserModel User
    {
        get;
        init;
    }

    public required AuthenticationType Type
    {
        get;
        init;
    }

    public required Byte[] StoredKey
    {
        get;
        set;
    }
}