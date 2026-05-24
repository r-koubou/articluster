using System.IO;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IImportNamingStrategy
{
    string GetInputFilePattern();
    SearchOption GetSearchOption();
}
