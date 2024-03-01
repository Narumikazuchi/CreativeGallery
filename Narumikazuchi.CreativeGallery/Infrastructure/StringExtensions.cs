namespace Narumikazuchi.CreativeGallery.Infrastructure;

static public class StringExtensions
{
    static public String ToPngImageSource(this String source)
    {
        return $"data:image/png;base64,{source}";
    }
}