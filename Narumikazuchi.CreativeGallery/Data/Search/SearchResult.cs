namespace Narumikazuchi.CreativeGallery.Data.Search;

public sealed record class SearchResult
{
    public required String Value
    {
        get;
        init;
    }

    public UInt32 Count
    {
        get;
        init;
    }

    public required SearchResultType Type
    {
        get;
        init;
    }
}