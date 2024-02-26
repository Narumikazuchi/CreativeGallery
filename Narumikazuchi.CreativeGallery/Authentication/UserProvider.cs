using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Authentication;

public sealed class UserProvider : IDisposable
{
    public UserProvider(UserDatabaseContext userDatabaseContext)
    {
        m_UserDatabaseContext = userDatabaseContext;
    }

    public void Dispose()
    {
        m_UserDatabaseContext.Dispose();

        GC.SuppressFinalize(obj: this);
    }

    public async ValueTask Load()
    {
        m_UserModel = await m_UserDatabaseContext.FindUserAsynchronously(user => user.DisplayName == "root");
    }

    public Boolean IsAuthenticatedUser
    {
        get
        {
            return true;
        }
    }

    public Optional<UserModel> AuthenticatedUser
    {
        get
        {
            return m_UserModel;
        }
    }

    private Optional<UserModel> m_UserModel = default;

    private readonly UserDatabaseContext m_UserDatabaseContext;
}