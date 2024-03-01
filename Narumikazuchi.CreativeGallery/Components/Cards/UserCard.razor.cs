using Microsoft.AspNetCore.Components;
using Narumikazuchi.CreativeGallery.Data.Users;
using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Components.Cards;

public sealed partial class UserCard
{
    [EditorRequired]
    [Parameter]
    public UserModel Model
    {
        get;
        set;
    } = null!;

    protected override void OnParametersSet()
    {
        Byte[] bytes = this.Model.ProfilePicture;
        String data = Convert.ToBase64String(inArray: bytes);
        this.ImageData = data.ToPngImageSource();
    }

    private void RedirectToResource()
    {
        this.Navigator.NavigateTo(uri: $"{Routes.USERDASHBOARD}{this.Model.DisplayName}");
    }

    private String ImageData
    {
        get;
        set;
    } = String.Empty;
}