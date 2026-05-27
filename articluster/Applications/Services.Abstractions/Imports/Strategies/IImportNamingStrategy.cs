using System.IO;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;

public interface IImportNamingStrategy
{
    string GetInputFilePattern();
    SearchOption GetSearchOption();
}
