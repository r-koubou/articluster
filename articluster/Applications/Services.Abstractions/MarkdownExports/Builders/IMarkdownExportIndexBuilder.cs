using System.Collections.Generic;

using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Builders;

public interface IMarkdownExportIndexBuilder
{
    string Build( string title, IReadOnlyCollection<ExportedFileEntry> entries, string markdownOutputDirectory );
}
