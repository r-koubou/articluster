using System.CommandLine;

using ArtiCluster.Applications.Cli;

var root = new RootCommand
{
    new CreateDefinitionCommandExecutor().CreateCommand(),
    new ConvertingCommandExecutor().CreateCommand()
};

return root.Parse( args ).Invoke();
