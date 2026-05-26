using System.Collections.Generic;

namespace ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Builders;

public interface IMarkdownProductIndexBuilder
{
    string Build( string manufacturerName, IReadOnlyCollection<string> productNames );
}
