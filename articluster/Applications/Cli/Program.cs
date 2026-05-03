using System.CommandLine;

using ArtiCluster.Applications.Cli;
using ArtiCluster.Applications.Services;
using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Commands
services.AddTransient<CreateDefinitionCommandExecutor>();
services.AddTransient<ConvertingCommandExecutor>();
// Converters
services.AddSingleton<IUniversalDefinitionLocalFileService, UniversalDefinitionLocalFileService>();
services.AddTransient<ILocalFileConversionService, CubaseLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, StudioOneLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, CakewalkLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, LogicLocalFileConversionService>();

using var serviceProvider = services.BuildServiceProvider();

var root = new RootCommand
{
    serviceProvider.GetRequiredService<CreateDefinitionCommandExecutor>().CreateCommand(),
    serviceProvider.GetRequiredService<ConvertingCommandExecutor>().CreateCommand()
};

return root.Parse( args ).Invoke();
