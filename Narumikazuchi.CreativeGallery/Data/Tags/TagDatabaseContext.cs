using Narumikazuchi.CreativeGallery.Data.Search;

namespace Narumikazuchi.CreativeGallery.Data.Tags;

public sealed class TagDatabaseContext : DatabaseContext
{
    public TagDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public async ValueTask AddTagAsynchronously(String tag,
                                                ExplicitRating rating,
                                                CancellationToken cancellationToken = default)
    {
        TagModel model = new()
        {
            Name = tag.ToLowerInvariant(),
            Rating = rating,
        };
        _ = await this.Context.Tags.AddAsync(entity: model,
                                             cancellationToken: cancellationToken);

        SearchResultModel searchResult = new()
        {
            Identifier = model.Identifier,
            Count = 0,
            Value = model.Name,
            Type = SearchResultType.Tag,
            ExtraParameters = rating.ToString(),
        };
        _ = await this.Context.SearchQuery.AddAsync(entity: searchResult,
                                                    cancellationToken: cancellationToken);
    }
}