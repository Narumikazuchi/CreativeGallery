using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data.Albums;

[PrimaryKey(nameof(Identifier))]
public sealed record class AlbumModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required String Name
    {
        get;
        set;
    }

    public required DataVisibility Visibility
    {
        get;
        set;
    }

    public UInt32 Likes
    {
        get
        {
            return (UInt32)this.LikedByUsers.Count;
        }
    }

    public DateTime CreatedAt
    {
        get;
        init;
    } = DateTime.Now;

    public DateTime LastModifiedAt
    {
        get;
        set;
    } = DateTime.Now;

    public List<CreativeWorkModel> Works
    {
        get;
        init;
    } = [];

    public List<UserModel> LikedByUsers
    {
        get;
        init;
    } = [];

    public List<UserModel> BookmarkedByUsers
    {
        get;
        init;
    } = [];

    public required Guid OwnerId
    {
        get;
        init;
    }

    public required UserModel Owner
    {
        get;
        init;
    }
}