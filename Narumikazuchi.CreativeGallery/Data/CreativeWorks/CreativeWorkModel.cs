using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Data.Tags;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data.CreativeWorks;

[PrimaryKey(nameof(Identifier))]
public sealed record class CreativeWorkModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required String Filename
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

    public List<TagModel> Tags
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

    public List<AlbumModel> PartOfAlbum
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