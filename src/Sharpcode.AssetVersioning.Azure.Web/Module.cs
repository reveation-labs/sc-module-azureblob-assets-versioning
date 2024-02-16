using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Module.Core;
using VirtoCommerce.Module.Core.Options;
using VirtoCommerce.Module.Core.Services;
using VirtoCommerce.Module.Data.Services;

namespace VirtoCommerce.Module.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        // Initialize database
        var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ??
                               Configuration.GetConnectionString("VirtoCommerce");
            
        serviceCollection.AddTransient<IAssetVersionService, AssetVersionService>();
        serviceCollection.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.SectionName));
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;
        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        // Register permissions
        var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "AssetVersions", ModuleConstants.Security.Permissions.AllPermissions);       

        // Apply migrations
        var serviceScope = serviceProvider.CreateScope();            
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
