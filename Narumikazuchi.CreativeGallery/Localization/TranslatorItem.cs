namespace Narumikazuchi.CreativeGallery.Localization;

public sealed class TranslatorItem
{
    public required CultureInfo Culture
    {
        get;
        init;
    }

    public required FrozenDictionary<String, String> Translations
    {
        get;
        init;
    }
}