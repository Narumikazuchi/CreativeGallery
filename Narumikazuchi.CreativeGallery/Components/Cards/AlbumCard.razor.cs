using Microsoft.AspNetCore.Components;
using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Components.Cards;

public sealed partial class AlbumCard
{
    [EditorRequired]
    [Parameter]
    public AlbumModel Model
    {
        get;
        set;
    } = null!;

    protected override void OnParametersSet()
    {
        ImmutableArray<String>.Builder builder = ImmutableArray.CreateBuilder<String>();

        Int32 limit = Math.Min(val1: 5,
                               val2: this.Model.Works.Count);

        for (Int32 index = 0;
             index < limit;
             index++)
        {
            Result<String> data = this.FileHandler.GetImageData(creativeWork: this.Model.Works[index]);
            if (data.IsOk is false)
            {
                continue;
            }

            String source = data.Value.ToPngImageSource();
            builder.Add(item: source);
        }

        this.ImageData = builder.ToImmutable();
    }

    private void RedirectToResource()
    {
        this.Navigator.NavigateTo(uri: $"{Routes.ALBUM}{this.Model.Identifier}");
    }

    private ImmutableArray<String> ImageData
    {
        get;
        set;
    }
}