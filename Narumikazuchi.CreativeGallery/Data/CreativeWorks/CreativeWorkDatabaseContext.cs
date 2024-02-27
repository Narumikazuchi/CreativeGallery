namespace Narumikazuchi.CreativeGallery.Data.CreativeWorks;

public sealed class CreativeWorkDatabaseContext : DatabaseContext
{
    public CreativeWorkDatabaseContext(GlobalDatabaseContext context,
                                       FileInputOutputHandler inputOutputHandler) :
        base(context: context)
    {
        m_InputOutputHandler = inputOutputHandler;
    }

    public async ValueTask AddCreativeWorkAsynchronously(CreativeWorkModel creativeWork,
                                                         CancellationToken cancellationToken = default)
    {
        _ = await this.Context.CreativeWorks.AddAsync(entity: creativeWork,
                                                      cancellationToken: cancellationToken);
    }

    private readonly FileInputOutputHandler m_InputOutputHandler;
}