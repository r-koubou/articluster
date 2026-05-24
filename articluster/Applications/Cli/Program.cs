using System.CommandLine;

using ArtiCluster.Applications.Cli;
using ArtiCluster.Applications.Services.Abstractions.Executors;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local;
using ArtiCluster.Applications.Services.Local.Executors;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using Serilog.Events;

var services = new ServiceCollection();

#region Setup Logging
const string logOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u4}] {SourceContext:l} {Message:lj}{NewLine}{Exception}";
var levelSwitch = new LoggingLevelSwitch();

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy( levelSwitch )
            .WriteTo.Console( outputTemplate: logOutputTemplate )
            .CreateLogger();

services.AddLogging( builder =>
    {
        builder.ClearProviders();
        builder.AddSerilog( dispose: true );
    }
);
#endregion ~Setup Logging

#region DI
// Commands
services.AddTransient<CreateDefinitionCommandExecutor>();
services.AddTransient<ConvertingCommandExecutor>();
// Converters
services.AddSingleton<IUniversalDefinitionFileService, UniversalDefinitionFileService>();
services.AddTransient<IExportFileService, CubaseExportFileService>();
services.AddTransient<IExportFileService, StudioOneExportFileService>();
services.AddTransient<IExportFileService, CakewalkExportFileService>();
services.AddTransient<IExportFileService, LogicExportFileService>();
// Executors
services.AddTransient<IUniversalDefinitionImportExecutor, UniversalDefinitionImportExecutor>();
#endregion ~DI

await using var serviceProvider = services.BuildServiceProvider();

#region Parsing Arguments

// Root -> Sub Commands
var root = new RootCommand
{
    serviceProvider.GetRequiredService<CreateDefinitionCommandExecutor>().CreateCommand(),
    serviceProvider.GetRequiredService<ConvertingCommandExecutor>().CreateCommand()
};

root.Description = "The tool to convert Universal Definition files into local file formats for various DAWs.";

// Global FLags
var verboseOption = new Option<bool>( "-v", "--verbose" )
{
    Description = "Enables verbose logging output"
};

root.Add( verboseOption );

var parseResult = root.Parse( args );

if( parseResult.GetValue( verboseOption ) )
{
    levelSwitch.MinimumLevel = LogEventLevel.Verbose;
}

return await parseResult.InvokeAsync();
#endregion ~Parsing Arguments
