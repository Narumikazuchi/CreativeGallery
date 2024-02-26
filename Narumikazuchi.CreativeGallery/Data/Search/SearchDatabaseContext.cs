using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.Search;

public sealed class SearchDatabaseContext : DatabaseContext
{
    public SearchDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public IAsyncEnumerable<SearchResultModel> FindWhereAsynchronously(String searchFilter,
                                                                       CancellationToken cancellationToken = default)
    {
        return this.Context.SearchQuery.Where(entity => EF.Functions.Like(entity.Value,
                                                                          $"%{searchFilter}%"))
                                       .AsAsyncEnumerable();
    }
}