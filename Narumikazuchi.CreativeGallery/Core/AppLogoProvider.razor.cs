namespace Narumikazuchi.CreativeGallery.Core;

public sealed partial class AppLogoProvider
{
    protected override void OnInitialized()
    {
        if (String.IsNullOrWhiteSpace(value: s_LogoData) is false)
        {
            return;
        }

        String fullpath = Path.Combine(this.Root.Directory.FullName, LOGO_FILENAME);
        using FileStream stream = File.OpenRead(path: fullpath);
        Byte[] bytes = new Byte[stream.Length];
        _ = stream.Read(buffer: bytes);
        String data = Convert.ToBase64String(inArray: bytes);
        s_LogoData = $"data:image/png;base64,{data}";

        base.OnInitialized();
    }

    static private String LogoData
    {
        get
        {
            return s_LogoData;
        }
    }

    static private String s_LogoData = String.Empty;

    private const String LOGO_FILENAME = "Logo.png";
}