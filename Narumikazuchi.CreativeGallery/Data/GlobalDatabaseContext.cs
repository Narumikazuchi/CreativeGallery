using Microsoft.EntityFrameworkCore;
using Narumikazuchi.CreativeGallery.Data.Albums;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Data.Permissions;
using Narumikazuchi.CreativeGallery.Data.Search;
using Narumikazuchi.CreativeGallery.Data.Tags;
using Narumikazuchi.CreativeGallery.Data.Users;

namespace Narumikazuchi.CreativeGallery.Data;

public sealed class GlobalDatabaseContext : DbContext
{
    public GlobalDatabaseContext(DbContextOptions<GlobalDatabaseContext> options) :
        base(options: options)
    {
        if (s_HasMigrated is false)
        {
            this.Database.Migrate();
            s_HasMigrated = true;
        }
    }

    public DbSet<AuthenticationModel> Authentications
    {
        get;
        set;
    }

    public DbSet<UserModel> Users
    {
        get;
        set;
    }

    public DbSet<PermissionModel> Permissions
    {
        get;
        set;
    }

    public DbSet<TagModel> Tags
    {
        get;
        set;
    }

    public DbSet<CreativeWorkModel> CreativeWorks
    {
        get;
        set;
    }

    public DbSet<AlbumModel> Albums
    {
        get;
        set;
    }

    public DbSet<SearchResultModel> SearchQuery
    {
        get;
        set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthenticationModel>()
                    .HasOne(entity => entity.User)
                    .WithMany(entity => entity.Authentications);

        modelBuilder.Entity<PermissionModel>()
                    .HasMany(entity => entity.Allows)
                    .WithMany(entity => entity.AllowedFor)
                    .UsingEntity<RestrictedByPermissionModel>(joinEntityName: RESTRICTED_BY_PERMISSIONS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.Permissions)
                    .WithMany(entity => entity.Users)
                    .UsingEntity<UserPermissionModel>(joinEntityName: USER_PERMISSIONS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.OwnedWorks)
                    .WithOne(entity => entity.Owner)
                    .HasForeignKey(entity => entity.OwnerIdentifier)
                    .HasPrincipalKey(entity => entity.Identifier);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.OwnedAlbums)
                    .WithOne(entity => entity.Owner)
                    .HasForeignKey(entity => entity.OwnerIdentifier)
                    .HasPrincipalKey(entity => entity.Identifier);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.LikedAlbums)
                    .WithMany(entity => entity.LikedByUsers)
                    .UsingEntity<UserLikedAlbumsModel>(joinEntityName: USER_LIKED_ALBUMS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.LikedWorks)
                    .WithMany(entity => entity.LikedByUsers)
                    .UsingEntity<UserLikedWorksModel>(joinEntityName: USER_LIKED_WORKS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.BookmarkedAlbums)
                    .WithMany(entity => entity.BookmarkedByUsers)
                    .UsingEntity<UserBookmarkedAlbumsModel>(joinEntityName: USER_BOOKMARKED_ALBUMS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.BookmarkedWorks)
                    .WithMany(entity => entity.BookmarkedByUsers)
                    .UsingEntity<UserBookmarkedWorksModel>(joinEntityName: USER_BOOKMARKED_WORKS_TABLE);

        modelBuilder.Entity<UserModel>()
                    .HasMany(entity => entity.FollowsUsers)
                    .WithMany(entity => entity.FollowedByUsers)
                    .UsingEntity<UserFollowsCreatorModel>(joinEntityName: USER_FOLLOWS_TABLE);

        modelBuilder.Entity<TagModel>()
                    .HasMany(entity => entity.TaggedWorks)
                    .WithMany(entity => entity.Tags)
                    .UsingEntity<CreativeWorkTaggedWithModel>(joinEntityName: WORKS_TAGGED_WITH_TABLE);

        modelBuilder.Entity<CreativeWorkModel>()
                    .HasMany(entity => entity.PartOfAlbum)
                    .WithMany(entity => entity.Works)
                    .UsingEntity<CreativeWorkInAlbumModel>(joinEntityName: WORKS_IN_ALBUM_TABLE);

        modelBuilder.Entity<SearchResultModel>()
                    .HasIndex(entity => entity.Value);
    }

    private const String RESTRICTED_BY_PERMISSIONS_TABLE = "RestrictedByPermissions";
    private const String USER_PERMISSIONS_TABLE = "UserPermissions";
    private const String USER_BOOKMARKED_ALBUMS_TABLE = "UserBookmarkedAlbums";
    private const String USER_BOOKMARKED_WORKS_TABLE = "UserBookmarkedWorks";
    private const String USER_LIKED_ALBUMS_TABLE = "UserLikedAlbums";
    private const String USER_LIKED_WORKS_TABLE = "UserLikedWorks";
    private const String USER_FOLLOWS_TABLE = "UserFollows";
    private const String WORKS_TAGGED_WITH_TABLE = "WorksTaggedWith";
    private const String WORKS_IN_ALBUM_TABLE = "WorksInAlbum";
}
