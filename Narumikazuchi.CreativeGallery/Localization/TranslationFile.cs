namespace Narumikazuchi.CreativeGallery.Localization;

public sealed class TranslationFile
{
    [JsonPropertyName("locale")]
    public required String Locale
    {
        get;
        init;
    }

    [JsonPropertyName("translations")]
    public required Dictionary<String, String> Translations
    {
        get;
        init;
    }
}