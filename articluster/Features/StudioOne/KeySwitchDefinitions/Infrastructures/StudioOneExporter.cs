using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons;
using ArtiCluster.Commons.IO;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Gateways;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures;

public sealed class StudioOneExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var mapResult = new StudioOneModelMapper().Map( source );

            if( mapResult.IsFailure )
            {
                return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError );
            }

            var rootElement = mapResult.Unwrap();
            var serializer = new XmlSerializer( typeof( StudioOneRootElement ) );

            // no xmlns adding
            // see: https://stackoverflow.com/a/8882612
            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add( "", "" );

            var stringWriter = new StringWriterWithEncoding( Encoding.UTF8 );
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true
            };

            using var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings );
            serializer.Serialize( xmlWriter, rootElement, xmlNamespaces );

            await writer.WriteAsync( stringWriter.ToString(), cancellationToken );

            return Result<Unit, ExportReason>.Success( Unit.Default );
        }
        catch( InvalidOperationException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError, e );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
        }
    }
}
