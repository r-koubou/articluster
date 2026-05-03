using System.CommandLine;

using ArtiCluster.Applications.Cli;
using ArtiCluster.Applications.Services;
using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

var services = new ServiceCollection();

#region Setup Logging
const string logOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u4}] {SourceContext:l} {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console( outputTemplate: logOutputTemplate )
            .CreateLogger();

services.AddLogging( builder =>
    {
        builder.ClearProviders();
        builder.AddSerilog( dispose: true );
    }
);
#endregion

#region DI
// Commands
services.AddTransient<CreateDefinitionCommandExecutor>();
services.AddTransient<ConvertingCommandExecutor>();
// Converters
services.AddSingleton<IUniversalDefinitionLocalFileService, UniversalDefinitionLocalFileService>();
services.AddTransient<ILocalFileConversionService, CubaseLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, StudioOneLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, CakewalkLocalFileConversionService>();
services.AddTransient<ILocalFileConversionService, LogicLocalFileConversionService>();
#endregion

using var serviceProvider = services.BuildServiceProvider();

var root = new RootCommand
{
    serviceProvider.GetRequiredService<CreateDefinitionCommandExecutor>().CreateCommand(),
    serviceProvider.GetRequiredService<ConvertingCommandExecutor>().CreateCommand()
};

return root.Parse( args ).Invoke();
