using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Components.Banner;

public sealed partial class UserLogin
{
    protected override async Task OnParametersSetAsync()
    {
        await this.UserProvider.Load();

        Byte[] bytes = this.UserProvider.AuthenticatedUser.Value!.ProfilePicture;
        String data = Convert.ToBase64String(inArray: bytes);
        this.ProfilePicture = $"data:image/png;base64,{data}";
        this.UserHasAdministrationAccess = true;
        this.Username = this.UserProvider.AuthenticatedUser.Value!.DisplayName;

        await base.OnParametersSetAsync();
    }

    private void RedirectToAdministration()
    {
        this.Navigator.NavigateTo(uri: Routes.ADMINISTRATION);
    }

    private void RedirectToUserDashboard()
    {
        this.Navigator.NavigateTo(uri: $"{Routes.USERDASHBOARD}{this.UserProvider.AuthenticatedUser.Value!.DisplayName}");
    }

    private void RedirectToBookmarks()
    {
        this.Navigator.NavigateTo(uri: Routes.BOOKMARKS);
    }

    private void RedirectToLiked()
    {
        this.Navigator.NavigateTo(uri: Routes.LIKED);
    }

    private void RedirectToFollowedCreators()
    {
        this.Navigator.NavigateTo(uri: Routes.FOLLOWED_CREATORS);
    }

    private void RedirectToSettings()
    {
        this.Navigator.NavigateTo(uri: Routes.SETTINGS);
    }

    private void RedirectToLogin()
    {
        this.Navigator.NavigateTo(uri: Routes.LOGIN);
    }

    private void Logout()
    {
        this.Navigator.NavigateTo(uri: Routes.LOGIN);
    }

    private void ToggleMenuOpen()
    {
        m_IsMenuOpen = !m_IsMenuOpen;
    }

    private Boolean UserHasAdministrationAccess
    {
        get;
        set;
    }

    private String ProfilePicture
    {
        get;
        set;
    } = String.Empty;

    private String Username
    {
        get;
        set;
    } = String.Empty;

    private String IsActiveClass
    {
        get
        {
            if (m_IsMenuOpen is true)
            {
                return ACTIVE_CLASS;
            }
            else
            {
                return String.Empty;
            }
        }
    }

    private Boolean m_IsMenuOpen = false;

    private const String ACTIVE_CLASS = "active";
}