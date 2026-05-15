using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Buffered;

using Semver;

using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        var yamlText = await reader.ReadAllAsync( cancellationToken );
        var deserializer = new DeserializerBuilder().Build();
        var dictionary = deserializer.Deserialize<Dictionary<object, object>>( yamlText );

        if( !dictionary.TryGetValue( IUniversalDefinitionModel.FormatVersionFieldName, out var formatVersion ) )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure(
                ImportFailureReason.UnsupportedFormatVersion,
                new UnsupportedFormatVersionException( "Missing 'FormatVersion' field." )
            );
        }

        var semVersion = SemVersion.Parse( formatVersion.ToString()! );
        var importer = FormatVersionResolver.ResolveImporter( semVersion );

        return await importer.ImportAsync( new TextContentReader( yamlText ), cancellationToken );
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        var exporter = FormatVersionResolver.GetLatestExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
