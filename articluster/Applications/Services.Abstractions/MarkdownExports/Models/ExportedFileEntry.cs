using System.IO;

namespace ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;

public sealed record ExportedFileEntry(
    string DawName,
    string ManufacturerName,
    string OutputDirectory,
    string OutputFileName,
    string DisplayName,
    string? GroupName = null,
    string? SubGroupName = null
)
{
    public string OutputPath
        => Path.Combine( OutputDirectory, OutputFileName );
}
