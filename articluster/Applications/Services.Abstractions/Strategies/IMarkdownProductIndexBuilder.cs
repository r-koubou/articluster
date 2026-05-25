using System.Collections.Generic;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IMarkdownProductIndexBuilder
{
    string Build( string manufacturerName, IReadOnlyCollection<string> productNames );
}
