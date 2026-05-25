using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Strategies;

namespace ArtiCluster.Applications.Services.Local;

public sealed class MarkdownExportIndexBuilder : IMarkdownExportIndexBuilder
{
    public string Build(
        string title,
        IReadOnlyCollection<ExportedFileEntry> entries,
        string markdownOutputDirectory )
    {
        if( entries.Count == 0 )
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        builder.AppendLine( $"# {title}" );
        builder.AppendLine();

        var groupedEntries =
            entries
               .OrderBy( x => x.GroupName ?? string.Empty )
               .ThenBy( x => x.SubGroupName ?? string.Empty )
               .ThenBy( x => x.DisplayName )
               .ThenBy( x => x.OutputFileName )
               .ToArray();

        var currentGroupName = default( string );
        var currentSubGroupName = default( string );

        foreach( var entry in groupedEntries )
        {
            var hasGroup = !string.IsNullOrWhiteSpace( entry.GroupName );
            var hasSubGroup = !string.IsNullOrWhiteSpace( entry.SubGroupName );

            if( hasGroup && currentGroupName != entry.GroupName )
            {
                if( currentGroupName is not null )
                {
                    builder.AppendLine();
                }

                builder.AppendLine( $"## {entry.GroupName}" );
                builder.AppendLine();

                currentGroupName    = entry.GroupName;
                currentSubGroupName = null;
            }

            if( hasSubGroup && currentSubGroupName != entry.SubGroupName )
            {
                if( currentSubGroupName is not null )
                {
                    builder.AppendLine();
                }

                builder.AppendLine( $"### {entry.SubGroupName}" );
                builder.AppendLine();

                currentSubGroupName = entry.SubGroupName;
            }

            var relativePath = Path.GetRelativePath(
                markdownOutputDirectory,
                entry.OutputPath
            ).Replace( '\\', '/' );

            builder.AppendLine( $"- [{entry.DisplayName}]({relativePath})" );
        }

        builder.AppendLine();

        return builder.ToString();
    }
}
