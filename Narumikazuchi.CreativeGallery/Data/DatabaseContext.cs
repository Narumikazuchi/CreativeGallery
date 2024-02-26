namespace Narumikazuchi.CreativeGallery.Data;

public abstract class DatabaseContext : IDisposable
{
    public DatabaseContext(GlobalDatabaseContext context)
    {
        this.Context = context;
    }

    public virtual void SaveChanges()
    {
        _ = this.Context.SaveChanges();
    }

    public void Dispose()
    {
        this.Context.Dispose();
        GC.SuppressFinalize(obj: this);
    }

    protected GlobalDatabaseContext Context
    {
        get;
    }
}