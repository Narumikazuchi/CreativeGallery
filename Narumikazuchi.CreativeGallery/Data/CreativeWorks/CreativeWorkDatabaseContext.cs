using Microsoft.EntityFrameworkCore;

namespace Narumikazuchi.CreativeGallery.Data.CreativeWorks;

public sealed class CreativeWorkDatabaseContext : DatabaseContext
{
    public CreativeWorkDatabaseContext(GlobalDatabaseContext context) :
        base(context: context)
    { }

    public async ValueTask AddCreativeWorkAsynchronously(CreativeWorkModel creativeWork,
                                                         CancellationToken cancellationToken = default)
    {
        _ = await this.Context.CreativeWorks.AddAsync(entity: creativeWork,
                                                      cancellationToken: cancellationToken);
    }

    public async Task<Optional<CreativeWorkModel>> LoadCreativeWorkAsynchronously(Guid identifier,
                                                                                  CancellationToken cancellationToken = default)
    {
        Optional<CreativeWorkModel> result = await this.Context.CreativeWorks.Include(entity => entity.Tags)
                                                                             .Include(entity => entity.LikedByUsers)
                                                                             .Include(entity => entity.BookmarkedByUsers)
                                                                             .Include(entity => entity.PartOfAlbum)
                                                                             .Include(entity => entity.Owner)
                                                                             .FirstOrDefaultAsync(entity => entity.Identifier == identifier,
                                                                                                  cancellationToken: cancellationToken);
        return result;
    }
}