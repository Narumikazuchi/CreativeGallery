namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

internal struct Rectangle
{
    public Double x;
    public Double y;
    public Double width;
    public Double height;

    internal Rectangle(Double x,
                       Double y,
                       Double width,
                       Double height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}