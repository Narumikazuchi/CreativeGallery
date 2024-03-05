namespace Narumikazuchi.CreativeGallery.Components.Container;

public sealed class CardData
{
    public Guid Identifier
    {
        get;
        init;
    }

    public CardDataType CardType
    {
        get;
        init;
    }
}