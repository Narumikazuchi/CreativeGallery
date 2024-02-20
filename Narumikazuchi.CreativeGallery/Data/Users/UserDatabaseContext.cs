using Narumikazuchi.CreativeGallery.Data.Permissions;

namespace Narumikazuchi.CreativeGallery.Data.Users;

public sealed class UserDatabaseContext : DatabaseContext
{
    public UserDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public void AddUser(UserModel user,
                        AuthenticationModel authentication)
    {
        _ = this.Context.Users.Add(entity: user);
        _ = this.Context.Authentications.Add(entity: authentication);
    }

    public void AddPermission(PermissionModel permission)
    {
        _ = this.Context.Permissions.Add(entity: permission);
    }

    public Boolean AuthenticateUser(UserModel user,
                                    AuthenticationType type,
                                    Byte[] key)
    {
        Optional<AuthenticationModel> authentication = this.Context.Authentications.FirstOrDefault(authentication => authentication.UserIdentifier == user.Identifier &&
                                                                                                                     authentication.Type == type);
        if (authentication.HasValue is false)
        {
            return false;
        }

        return key.SequenceEqual(second: authentication.Value.StoredKey) is true;
    }

    public Optional<UserModel> FindUser(Expression<Func<UserModel, Boolean>> predicate)
    {
        Optional<UserModel> result = this.Context.Users.FirstOrDefault(predicate: predicate);
        return result;
    }

    public Optional<PermissionModel> FindPermission(Expression<Func<PermissionModel, Boolean>> predicate)
    {
        Optional<PermissionModel> result = this.Context.Permissions.FirstOrDefault(predicate: predicate);
        return result;
    }
}