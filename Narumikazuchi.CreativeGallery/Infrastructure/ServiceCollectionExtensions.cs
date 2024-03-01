using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narumikazuchi.CreativeGallery.Authentication;
using Narumikazuchi.CreativeGallery.Data;
using Narumikazuchi.CreativeGallery.Data.Search;
using Narumikazuchi.CreativeGallery.Data.Users;
using Narumikazuchi.CreativeGallery.Localization;

namespace Narumikazuchi.CreativeGallery.Infrastructure;

static public class ServiceCollectionExtensions
{
    static public void AddWorkingDirectory(this IServiceCollection services)
    {
        if (Environment.ProcessPath is not null)
        {
            Optional<DirectoryInfo> directory = new FileInfo(fileName: Environment.ProcessPath).Directory;
            if (directory.HasValue is false)
            {
                directory = new DirectoryInfo(path: Environment.CurrentDirectory);
            }

            services.AddSingleton(implementationInstance: new WorkingDirectory()
            {
                Directory = directory.Value!,
            });
        }
        else
        {
            services.AddSingleton(implementationInstance: new WorkingDirectory()
            {
                Directory = new DirectoryInfo(path: Environment.CurrentDirectory),
            });
        }
    }

    static public void AddDatabase(this IServiceCollection services,
                                   IConfiguration configuration)
    {
        IConfigurationSection section = configuration.GetSection(key: DATABASE_KEY);
        services.AddDbContext<GlobalDatabaseContext>(optionsAction: options => options.UseSqlite(connectionString: section.GetValue<String>(key: CONNECTION_STRING_KEY)),
                                                     contextLifetime: ServiceLifetime.Transient,
                                                     optionsLifetime: ServiceLifetime.Transient);
        services.AddTransient<UserDatabaseContext>();
        services.AddTransient<SearchDatabaseContext>();
    }

    static public void AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<FileInputOutputHandler>();
        services.AddTransient<Translator>();
        services.AddTransient<UserProvider>();
    }

    private const String DATABASE_KEY = "Database";
    private const String CONNECTION_STRING_KEY = "ConnectionString";
}