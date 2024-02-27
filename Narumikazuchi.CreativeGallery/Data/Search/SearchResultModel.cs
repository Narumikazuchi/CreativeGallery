using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Search;

[PrimaryKey(nameof(Identifier))]
public sealed record class SearchResultModel
{
    public required Guid Identifier
    {
        get;
        init;
    }

    public required String Value
    {
        get;
        set;
    }

    public UInt32 Count
    {
        get;
        set;
    }

    public required SearchResultType Type
    {
        get;
        init;
    }

    public String ExtraParameters
    {
        get;
        set;
    } = String.Empty;
}