using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narumikazuchi.CreativeGallery.Data;
using Narumikazuchi.CreativeGallery.Data.Permissions;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Infrastructure;

static public class WebApplicationExtensions
{
    static public async ValueTask RunInitialSetupAsynchronously(this WebApplication application)
    {
        String rootUser;
        String rootPassword;
        using UserDatabaseContext databaseContext = application.Services.GetRequiredService<UserDatabaseContext>();

        IConfigurationSection section = application.Configuration.GetSection(key: INITIAL_USER_KEY);
        Optional<String> value = section.GetValue<String>(key: ROOT_USER_KEY);
        if (value.HasValue is false)
        {
            throw new NullReferenceException(message: String.Format(format: KEY_NOT_FOUND,
                                                                    arg0: ROOT_USER_KEY));
        }
        else
        {
            rootUser = value.Value;
        }

        value = section.GetValue<String>(key: ROOT_PASSWORD_KEY);
        if (value.HasValue is false)
        {
            throw new NullReferenceException(message: String.Format(format: KEY_NOT_FOUND,
                                                                    arg0: ROOT_PASSWORD_KEY));
        }
        else
        {
            rootPassword = value.Value;
        }

        UserModel root = await EnsureRootUserAsynchronously(rootUser: rootUser,
                                                            rootPassword: rootPassword,
                                                            databaseContext: databaseContext);
        await EnsureRootPermissionsAsynchronously(rootUser: root,
                                                  databaseContext: databaseContext);

        databaseContext.SaveChanges();
    }

    static private async Task<UserModel> EnsureRootUserAsynchronously(String rootUser,
                                                                      String rootPassword,
                                                                      UserDatabaseContext databaseContext)
    {
        Optional<UserModel> root = await databaseContext.FindUserAsynchronously(user => user.DisplayName == rootUser);
        if (root.HasValue is true)
        {
            return root.Value;
        }

        Byte[] bytes = PasswordToBytes(password: rootPassword);
        Byte[] hash = SHA512.HashData(source: bytes);

        UserModel user = new()
        {
            Username = rootUser,
            DisplayName = rootUser,
            Email = String.Empty,
            Visibility = DataVisibility.Private,
            ProfilePicture = null!,
        };

        AuthenticationModel authentication = new()
        {
            User = user,
            UserIdentifier = user.Identifier,
            Type = AuthenticationType.Password,
            StoredKey = hash
        };

        await databaseContext.AddUserAsynchronously(user: user,
                                                    authentication: authentication);
        return user;
    }

    static private async ValueTask EnsureRootPermissionsAsynchronously(UserModel rootUser,
                                                                       UserDatabaseContext databaseContext)
    {
        Optional<PermissionModel> root = await databaseContext.FindPermissionAsynchronously(permission => permission.Name == ROOT_PERMISSIONS);
        if (root.HasValue is true)
        {
            return;
        }

        PermissionModel permission = new()
        {
            Name = ROOT_PERMISSIONS,
            Users = [rootUser],
            CanBeDeleted = false
        };

        await databaseContext.AddPermissionAsynchronously(permission: permission);
    }

    static private Byte[] PasswordToBytes(String password)
    {
        ReadOnlySpan<Byte> span = MemoryMarshal.AsBytes<Char>(span: password);
        return span.ToArray();
    }

    private const String ROOT_PERMISSIONS = "root";
    private const String INITIAL_USER_KEY = "InitialRootUser";
    private const String ROOT_USER_KEY = "Username";
    private const String ROOT_PASSWORD_KEY = "Password";

    private const String KEY_NOT_FOUND = "The configuration key '{0}' was not found in the appsettings.json.";
}