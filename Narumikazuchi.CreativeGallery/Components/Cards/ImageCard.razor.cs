using Microsoft.AspNetCore.Components;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Components.Cards;

public sealed partial class ImageCard
{
    [EditorRequired]
    [Parameter]
    public CreativeWorkModel Model
    {
        get;
        set;
    } = null!;

    protected override void OnParametersSet()
    {
        Result<String> data = this.FileHandler.GetImageData(creativeWork: this.Model);
        if (data.IsOk is false)
        {
            return;
        }

        this.ImageData = data.Value.ToPngImageSource();
    }

    private void RedirectToResource()
    {
        this.Navigator.NavigateTo(uri: $"{Routes.CREATIVE_WORK}{this.Model.Identifier}");
    }

    private String ImageData
    {
        get;
        set;
    } = String.Empty;
}