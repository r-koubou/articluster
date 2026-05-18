using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.Imports;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        try
        {
            // [NOTE]
            // Now, only have one importer, so we ignore the formatVersion.
            // In the future, if we have multiple importers, we'll need to determine the formatVersion first (probably by peeking at the content)
            // and then resolve the appropriate importer.
#if false
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

            var importer = FormatVersionResolver.ResolveImporter( int.Parse( formatVersion ) );
#endif
            var importer = new YamlImporter();
            return await importer.ImportAsync( reader, cancellationToken );
        }
        catch( YamlException e )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure( ImportFailureReason.DeserializationError, e );
        }
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        var exporter = FormatVersionResolver.GetLatestExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
