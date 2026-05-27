using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Builders;

namespace ArtiCluster.Applications.Services.Local.Exports.MarkdownExports.Builders;

public sealed class DefaultMarkdownProductIndexBuilder : IMarkdownProductIndexBuilder
{
    public string Build(
        string manufacturerName,
        IReadOnlyCollection<string> productNames )
    {
        if( productNames.Count == 0 )
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        builder.AppendLine( $"# {manufacturerName}" );
        builder.AppendLine();

        foreach( var productName in productNames.Distinct().OrderBy( x => x ) )
        {
            builder.AppendLine( $"- [{productName}](./{productName}.md)" );
        }

        builder.AppendLine();

        return builder.ToString();
    }
}
