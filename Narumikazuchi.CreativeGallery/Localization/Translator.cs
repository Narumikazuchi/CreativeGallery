using Microsoft.Extensions.Configuration;
using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Localization;

public sealed class Translator
{
    public Translator(IConfiguration configuration,
                      WorkingDirectory workingDirectory)
    {
        if (s_Fallback is not null &&
            s_Translations is not null)
        {
            this.CurrentLocale = s_Fallback;
            return;
        }

        IConfigurationSection section = configuration.GetSection(key: LOCALIZATION_KEY);
        Optional<String> fallbackValue = section.GetValue<String>(key: FALLBACK_KEY);
        if (fallbackValue.HasValue is false)
        {
            throw new NullReferenceException(message: $"Fallback for translation was not provided. Please provide the fallback in the appsettings under '{LOCALIZATION_KEY}:{FALLBACK_KEY}'.");
        }

        try
        {
            _ = new CultureInfo(name: fallbackValue.Value);
            s_Fallback = fallbackValue.Value;
        }
        catch
        {
            throw new InvalidOperationException(message: "Fallback for translation not a valid locale format. Please use a standard 'locale-COUNTRY' format where 'locale' is an ISO 639 two-letter language code and 'COUNTRY' is an ISO 3166 two-letter country code (i.e. 'en-GB').");
        }

        Optional<String> directoryPath = section.GetValue<String>(key: DIRECTORY_KEY);
        if (directoryPath.HasValue is false)
        {
            throw new NullReferenceException(message: $"Directory for translation files was not provided. Please provide the directory in the appsettings under '{LOCALIZATION_KEY}:{DIRECTORY_KEY}'.");
        }

        DirectoryInfo directory = new(path: Path.Combine(workingDirectory.Directory.FullName, directoryPath.Value));
        if (directory.Exists is false)
        {
            throw new DirectoryNotFoundException(message: "The directory for translation files does not exist. Please provide a valid path to an existing directory with translation files.");
        }

        Dictionary<String, TranslatorItem> items = [];
        IEnumerable<FileInfo> files = directory.EnumerateFiles(searchPattern: JSON_FILE_PATTERN);
        foreach (FileInfo file in files)
        {
            try
            {
                using FileStream stream = file.OpenRead();
                Optional<TranslationFile> translations = JsonSerializer.Deserialize<TranslationFile>(utf8Json: stream);
                if (translations.HasValue is true)
                {
                    if (items.TryGetValue(key: translations.Value.Locale,
                                          value: out TranslatorItem item) is true)
                    {
                        item = new()
                        {
                            Culture = item.Culture,
                            Translations = item.Translations.Concat(second: translations.Value.Translations)
                                                            .ToFrozenDictionary(),
                        };
                        items[translations.Value.Locale] = item;
                    }
                    else
                    {
                        CultureInfo culture = new(name: translations.Value.Locale);
                        item = new()
                        {
                            Culture = culture,
                            Translations = translations.Value.Translations.ToFrozenDictionary(),
                        };
                        items.Add(key: translations.Value.Locale,
                                  value: item);
                    }
                }
            }
            catch
            { }
        }

        s_Translations = items.ToFrozenDictionary();

        this.CurrentLocale = s_Fallback;
    }

    public String CurrentLocale
    {
        get;
        set;
    }

    public CultureInfo CurrentCulture
    {
        get
        {
            if (s_Translations.TryGetValue(key: this.CurrentLocale,
                                           value: out TranslatorItem item) is true)
            {
                return item.Culture;
            }
            else if (s_Translations.TryGetValue(key: s_Fallback,
                                                   value: out item) is true)
            {
                return item.Culture;
            }
            else
            {
                return new (name: s_Fallback);
            }
        }
    }

    public String this[String key]
    {
        get
        {
            if (s_Translations.TryGetValue(key: this.CurrentLocale,
                                           value: out TranslatorItem item) is true)
            {
                if (item.Translations.TryGetValue(key: key,
                                                  value: out String? value) is true)
                {
                    return value;
                }
                else if(s_Translations.TryGetValue(key: s_Fallback,
                                                   value: out item) is true)
                {
                    if (item.Translations.TryGetValue(key: key,
                                                      value: out value) is true)
                    {
                        return value;
                    }
                    else
                    {
                        return NO_TRANSLATION;
                    }
                }
                else
                {
                    return NO_TRANSLATION;
                }
            }
            else if (s_Translations.TryGetValue(key: s_Fallback,
                                                   value: out item) is true)
            {
                if (item.Translations.TryGetValue(key: key,
                                                  value: out String? value) is true)
                {
                    return value;
                }
                else
                {
                    return NO_TRANSLATION;
                }
            }
            else
            {
                return NO_TRANSLATION;
            }
        }
    }

    static private String s_Fallback = null!;
    static private FrozenDictionary<String, TranslatorItem> s_Translations = null!;

    private const String LOCALIZATION_KEY = "Localization";
    private const String FALLBACK_KEY = "Fallback";
    private const String DIRECTORY_KEY = "Directory";
    private const String JSON_FILE_PATTERN = "*.json";
    private const String NO_TRANSLATION = "No Translation!";
}