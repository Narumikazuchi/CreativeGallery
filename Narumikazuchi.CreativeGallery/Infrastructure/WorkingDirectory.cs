namespace Narumikazuchi.CreativeGallery.Infrastructure;

public sealed record class WorkingDirectory
{
    public required DirectoryInfo Directory
    {
        get;
        init;
    }
}