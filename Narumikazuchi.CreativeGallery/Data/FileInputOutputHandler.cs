using Microsoft.Extensions.Configuration;
using Narumikazuchi.CreativeGallery.Data.CreativeWorks;
using Narumikazuchi.CreativeGallery.Data.Users;
using Narumikazuchi.CreativeGallery.Infrastructure;
using Narumikazuchi.CreativeGallery.Plugins.Imaging;

namespace Narumikazuchi.CreativeGallery.Data;

public sealed class FileInputOutputHandler
{
    public FileInputOutputHandler(IConfiguration configuration,
                                  WorkingDirectory workingDirectory)
    {
        if (s_Root is not null)
        {
            return;
        }

        IConfigurationSection section = configuration.GetSection(key: DATABASE_SECTION_KEY);
        section = section.GetSection(key: DATALOCATION_SECTION_KEY);

        Optional<String> path = section.GetValue<String>(key: PATH_KEY);
        if (path.HasValue is false)
        {
            throw new Exception();
        }

        Optional<Boolean> isRelative = section.GetValue<Boolean>(key: IS_REALTIVE_KEY);
        String fullpath;
        if (isRelative.HasValue is true &&
            isRelative.Value is false)
        {
            fullpath = path.Value;
        }
        else
        {
            fullpath = Path.Combine(workingDirectory.Directory.FullName, path.Value);
        }

        DirectoryInfo directory = new(path: fullpath);
        if (directory.Exists is false)
        {
            if (isRelative.HasValue is false ||
                isRelative.Value is false)
            {
                throw new Exception();
            }
            else
            {
                directory = Directory.CreateDirectory(path: fullpath);
            }
        }

        s_Root = directory;
    }

    public async ValueTask StoreCreativeWork(CreativeWorkModel creativeWork,
                                             ImageFile image,
                                             UserModel owner)
    {
        String fullpath = Path.Combine(s_Root.FullName, owner.Identifier.ToString());
        if (Directory.Exists(path: fullpath) is false)
        {
            _ = Directory.CreateDirectory(path: fullpath);
        }

        fullpath = Path.Combine(fullpath, creativeWork.Filename);
        await using FileStream stream = File.Create(path: fullpath);
        Optional<Exception> exception = image.SaveInto(stream: stream,
                                                       format: ImageFormat.Png);
        if (exception.HasValue is true)
        {
            throw exception.Value;
        }

        if (creativeWork.Filename.StartsWith(value: PROFILE_PREFIX) is false &&
            (image.Width > THUMBNAIL_MAX_WIDTH ||
            image.Height > THUMBNAIL_MAX_HEIGHT))
        {
            (Double width, Double height) = (THUMBNAIL_MAX_WIDTH, THUMBNAIL_MAX_HEIGHT);
            if (image.Width > image.Height)
            {
                height *= (Double)image.Height / image.Width;
            }
            else if (image.Height > image.Width)
            {
                width *= (Double)image.Width / image.Height;
            }

            Result<ImageFile> thumbnail = image.GetThumbnailImage((Int32)width, (Int32)height);
            if (thumbnail.IsOk is true)
            {
                await using FileStream thumbnailStream = File.Create($"{fullpath}.thumbnail");
                thumbnail.Value.SaveInto(stream: thumbnailStream,
                                         format: ImageFormat.Png);
            }
            else if (thumbnail.IsError is true)
            {
                throw thumbnail.Error;
            }
        }
    }

    static private DirectoryInfo s_Root = null!;

    private const String DATABASE_SECTION_KEY = "Database";
    private const String DATALOCATION_SECTION_KEY = "DataLocation";
    private const String PATH_KEY = "Path";
    private const String IS_REALTIVE_KEY = "IsRelative";
    private const String PROFILE_PREFIX = "profile_";
    private const Int32 THUMBNAIL_MAX_WIDTH = 320;
    private const Int32 THUMBNAIL_MAX_HEIGHT = 320;
}