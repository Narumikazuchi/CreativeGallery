using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;

namespace Narumikazuchi.CreativeGallery.Data.Tags;

[PrimaryKey(nameof(Identifier))]
public sealed record class TagModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required String Name
    {
        get;
        init;
    }

    public DateTime CreatedAt
    {
        get;
        init;
    } = DateTime.Now;

    public UInt32 Count
    {
        get
        {
            return (UInt32)this.TaggedWorks.Count;
        }
    }

    public List<CreativeWorkModel> TaggedWorks
    {
        get;
        init;
    } = [];
}