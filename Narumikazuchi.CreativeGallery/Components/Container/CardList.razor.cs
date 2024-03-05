using Microsoft.AspNetCore.Components;
using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Components.Container;

public sealed partial class CardList : IDisposable
{
    public void Dispose()
    {
        this.UsersContext.Dispose();
        this.CreativeWorksContext.Dispose();
        this.AlbumsContext.Dispose();
    }

    [EditorRequired]
    [Parameter]
    public required ImmutableArray<CardData> Data
    {
        get;
        set;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (this.Data.IsDefaultOrEmpty is true)
        {
            return;
        }

        foreach (CardData data in this.Data)
        {
            switch (data.CardType)
            {
                case CardDataType.User:
                    Optional<UserModel> user = await this.UsersContext.LoadUserAsynchronously(identifier: data.Identifier);
                    if (user.HasValue is true)
                    {
                        m_Models.Add(item: user.Value);
                    }

                    break;
                case CardDataType.CreativeWork:
                    Optional<CreativeWorkModel> creativeWork = await this.CreativeWorksContext.LoadCreativeWorkAsynchronously(identifier: data.Identifier);
                    if (creativeWork.HasValue is true)
                    {
                        m_Models.Add(item: creativeWork.Value);
                    }

                    break;
                case CardDataType.Album:
                    Optional<AlbumModel> album = await this.AlbumsContext.LoadAlbumAsynchronously(identifier: data.Identifier);
                    if (album.HasValue is true)
                    {
                        m_Models.Add(item: album.Value);
                    }

                    break;
            }
        }
    }

    private readonly List<Object> m_Models = [];
}