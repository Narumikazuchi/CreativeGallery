namespace Narumikazuchi.CreativeGallery.Localization;

public readonly struct TranslatorItem
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