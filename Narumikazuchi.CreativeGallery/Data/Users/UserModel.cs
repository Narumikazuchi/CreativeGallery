using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Data.Permissions;

namespace Narumikazuchi.CreativeGallery.Data.Users;

[PrimaryKey(nameof(Identifier))]
public sealed record class UserModel
{
    public Guid Identifier
    {
        get;
        init;
    } = Guid.NewGuid();

    public required String Username
    {
        get;
        init;
    }

    public required String DisplayName
    {
        get;
        set;
    }

    public required String Email
    {
        get;
        set;
    }

    public required DataVisibility Visibility
    {
        get;
        set;
    }

    public UInt32 Followers
    {
        get
        {
            return (UInt32)this.FollowedByUsers.Count;
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

    public List<AuthenticationModel> Authentications
    {
        get;
        init;
    } = [];

    public List<PermissionModel> Permissions
    {
        get;
        init;
    } = [];

    public List<UserModel> FollowsUsers
    {
        get;
        init;
    } = [];

    public List<UserModel> FollowedByUsers
    {
        get;
        init;
    } = [];

    public List<CreativeWorkModel> OwnedWorks
    {
        get;
        init;
    } = [];

    public List<AlbumModel> OwnedAlbums
    {
        get;
        init;
    } = [];

    public List<CreativeWorkModel> LikedWorks
    {
        get;
        init;
    } = [];

    public List<AlbumModel> LikedAlbums
    {
        get;
        init;
    } = [];

    public List<CreativeWorkModel> BookmarkedWorks
    {
        get;
        init;
    } = [];

    public List<AlbumModel> BookmarkedAlbums
    {
        get;
        init;
    } = [];
}