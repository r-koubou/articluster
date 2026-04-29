using System.CommandLine;

using ArtiCluster.Applications.Cli;
using ArtiCluster.Applications.Services;
using ArtiCluster.Applications.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Commands
services.AddTransient<CreateDefinitionCommandExecutor>();
services.AddTransient<ConvertingCommandExecutor>();
// Converters
services.AddTransient<ILocalFileConvertingService, CubaseLocalFileConvertingService>();
services.AddTransient<ILocalFileConvertingService, StudioOneLocalFileConvertingService>();
services.AddTransient<ILocalFileConvertingService, CakewalkLocalFileConvertingService>();
services.AddTransient<ILocalFileConvertingService, LogicLocalFileConvertingService>();

using var serviceProvider = services.BuildServiceProvider();

var root = new RootCommand
{
    serviceProvider.GetRequiredService<CreateDefinitionCommandExecutor>().CreateCommand(),
    serviceProvider.GetRequiredService<ConvertingCommandExecutor>().CreateCommand()
};

return root.Parse( args ).Invoke();
