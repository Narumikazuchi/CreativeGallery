using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Data.Tags;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data.Search;

public sealed class SearchDatabaseContext
{
    public SearchDatabaseContext(GlobalDatabaseContext context)
    {
        m_Context = context;
    }

    public ImmutableArray<SearchResult> Find(String searchFilter)
    {
        searchFilter = searchFilter.ToLowerInvariant();

        ImmutableArray<SearchResult>.Builder builder = ImmutableArray.CreateBuilder<SearchResult>();

        IQueryable<TagModel> tagQuery = m_Context.Tags.Where(predicate: tag => tag.Name.Contains(searchFilter));
        foreach (TagModel tag in tagQuery)
        {
            SearchResult result = new()
            {
                Value = tag.Name,
                Type = SearchResultType.Tag,
                Count = tag.Count,
            };
            builder.Add(item: result);
        }

        IQueryable<UserModel> userQuery = m_Context.Users.Where(predicate: user => user.DisplayName.Contains(searchFilter));
        foreach (UserModel user in userQuery)
        {
            SearchResult result = new()
            {
                Value = user.DisplayName,
                Type = SearchResultType.User,
                Count = user.Followers,
            };
            builder.Add(item: result);
        }

        IQueryable<AlbumModel> albumQuery = m_Context.Albums.Where(predicate: album => album.Name.Contains(searchFilter));
        foreach (AlbumModel album in albumQuery)
        {
            SearchResult result = new()
            {
                Value = album.Name,
                Type = SearchResultType.Album,
                Count = album.Likes
            };
            builder.Add(item: result);
        }

        Int32 Sort(SearchResult left,
                   SearchResult right)
        {
            Int32 leftScore = left.Value.IndexOf(value: searchFilter);
            Int32 rightScore = right.Value.IndexOf(value: searchFilter);

            Int32 result = leftScore.CompareTo(value: rightScore);
            if (result is not 0)
            {
                return result;
            }
            else
            {
                return left.Count.CompareTo(value: right.Count);
            }
        }

        builder.Sort(comparison: Sort);

        return builder.ToImmutable();
    }

    private readonly GlobalDatabaseContext m_Context;
}