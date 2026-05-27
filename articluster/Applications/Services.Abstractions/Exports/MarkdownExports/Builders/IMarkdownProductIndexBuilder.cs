using System.Collections.Generic;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Builders;

public interface IMarkdownProductIndexBuilder
{
    string Build( string manufacturerName, IReadOnlyCollection<string> productNames );
}
