using System.CommandLine;

namespace ArtiCluster.Applications.Cli;

public interface ICommandExecutor
{
    Command CreateCommand();
}
