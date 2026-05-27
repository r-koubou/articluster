using System.Collections.Generic;

using ArtiCluster.Applications.Services.Abstractions.Exports.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Builders;

public interface IMarkdownExportIndexBuilder
{
    string Build( string title, IReadOnlyCollection<ExportedFileEntry> entries, string markdownOutputDirectory );
}
