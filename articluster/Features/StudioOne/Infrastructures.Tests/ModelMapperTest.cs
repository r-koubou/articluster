using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons;
using ArtiCluster.Commons.IO;
using ArtiCluster.Features.StudioOne.Gateways;
using ArtiCluster.Features.StudioOne.Infrastructures.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using NUnit.Framework;

namespace ArtiCluster.Features.StudioOne.Infrastructures.Tests;

[TestFixture]
public class ModelMapperTest
{
    [Test]
    public void MapsRootModelToArticulationTest()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var source1 = CreateMock( id1 );
        var source2 = CreateMock( id2, "E.Bass" );

        var actual = new StudioOneModelMapper().Map( [ source1, source2 ] );

        Assert.That( actual.IsSuccess, Is.True, "Mapping should succeed" );

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
        serializer.Serialize( xmlWriter, actual.Unwrap(), xmlNamespaces );

        TestContext.Out.WriteLine( stringWriter.ToString() );

        Assert.Multiple( () => {} );
    }

    private static UniversalDefinition CreateMock( Guid id, string patchName = "Epic Lead" )
    {
        return new UniversalDefinition(
            id: id,
            author: "John Doe",
            manufacturerName: "Acme Corp",
            productName: "Super Synth",
            patchName: patchName,
            description: "multi-line\ndescription",
            extra: new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
            articulations:
            [
                new Articulation(
                    name: "Sustain",
                    midiMessages:
                    [
                        // Note On
                        new MidiMessage( 0x90, 40, 100 ),
                        // Note Off
                        new MidiMessage( 0x80, 40, 110 ),
                        // Control Change
                        new MidiMessage( 0xB0, 1, 127 ),
                        // Program Change
                        new MidiMessage( 0xC0, 49 ),
                    ],
                    extra: new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                )
            ]
        );
    }
}

public interface IStudioOneModelMapper
{
    Result<StudioOneRootElement, ExportReason> Map( IReadOnlyCollection<UniversalDefinition> combinedSources );
}

public sealed class StudioOneModelMapper : IStudioOneModelMapper
{
    public Result<StudioOneRootElement, ExportReason> Map( IReadOnlyCollection<UniversalDefinition> combinedSources )
    {
        if( combinedSources.Count == 0 )
        {
            return Result<StudioOneRootElement, ExportReason>.Success( new StudioOneRootElement() );
        }
        
        var validationResult = UniversalDefinition.IsCombinedWithSameManufacturerAndProduct( combinedSources );

        if( !validationResult )
        {
            return Result<StudioOneRootElement, ExportReason>.Failure( ExportReason.MixedPatchDefinitionsError );
        }
        
        var first = combinedSources.First();
        var groupedByPatches =
            UniversalDefinition.GroupByPatchName(
                combinedSources,
                first.ManufacturerName,
                first.ProductName
            );
        
        var assignId = 0;
        
        // Create with folder element if several patches exist
        if( groupedByPatches.Count >= 2 )
        {
            return MapWithFolders( groupedByPatches, assignId );
        }

#if false
    <?xml version="1.0" encoding="utf-8"?>
       <Music.KeySwitchList name="Super Synth">
         <Attributes name="Sustain" id="0" pitch="40" momentary="0" activation="note40.100|off40.110|cc1.127|pc49" />
         <Attributes name="Sustain" id="1" pitch="40" momentary="0" activation="note40.100|off40.110|cc1.127|pc49" />
       </Music.KeySwitchList>
#endif
        var rootElement = new StudioOneRootElement
        {
            Name = $"{first.ProductName.Value} {first.PatchName.Value}"
        };

        var attributeElements = MapElementAttributes( combinedSources, ref assignId );
        rootElement.AttributeElements.AddRange( attributeElements );

        return Result<StudioOneRootElement, ExportReason>.Success( rootElement );
    }

    private static Result<StudioOneRootElement, ExportReason> MapWithFolders( List<List<UniversalDefinition>> groupedByPatches, int assignId )
    {
#if false
    <?xml version="1.0" encoding="utf-8"?>
       <Music.KeySwitchList name="Super Synth">
         <Attributes folder="1" name="Epic Lead">
           <Attributes name="Sustain" id="0" pitch="40" momentary="0" activation="note40.100|off40.110|cc1.127|pc49" />
         </Attributes>
         <Attributes folder="1" name="E.Bass">
           <Attributes name="Sustain" id="1" pitch="40" momentary="0" activation="note40.100|off40.110|cc1.127|pc49" />
         </Attributes>
       </Music.KeySwitchList>
 #endif
        var first = groupedByPatches[ 0 ].First();

        var rootElement = new StudioOneRootElement
        {
            Name = $"{first.ProductName.Value}"
        };

        foreach( var patches in groupedByPatches )
        {
            if( patches.Count == 0 )
            {
                continue;
            }

            var folder = new AttributeElement
            {
                Folder = "1",
                Name   = patches.First().PatchName.Value
            };

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach( var definition in patches )
            {
                foreach( var articulation in definition.Articulations )
                {
                    var attributeElement = MapElementAttribute( articulation, assignId );
                    folder.Children.Add( attributeElement );
                    assignId++;
                }
            }

            rootElement.AttributeElements.Add( folder );
        }

        return Result<StudioOneRootElement, ExportReason>.Success( rootElement );
    }

    private static List<AttributeElement> MapElementAttributes( IReadOnlyCollection<UniversalDefinition> sources, ref int assignId )
    {
        var result = new List<AttributeElement>();

        foreach( var definition in sources )
        {
            foreach( var articulation in definition.Articulations )
            {
                var attr = MapElementAttribute( articulation, assignId );
                assignId++;

                result.Add( attr );
            }
        }

        return result;
    }

    private static AttributeElement MapElementAttribute( Articulation articulation, int assignId )
    {
        var name = articulation.Name.Value;
        var pitch = AttributeElement.NoPitch;
        var activation = MapActivation( articulation );

        var midiNoteOns = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOn ).ToList();

        if( midiNoteOns.Count != 0 )
        {
            pitch = midiNoteOns.Single().Data1.Value;
        }

        string? color = null;

        if( articulation.Extra.TryGetValue( ExtraDataKeys.Color, out var value ) )
        {
            color = value;
        }

        var momentary = 0;

        if( articulation.Extra.TryGetValue( ExtraDataKeys.Momentary, out var momentaryValue ) )
        {
            momentary = momentaryValue == "0" ? 0 : 1;
        }

        return new AttributeElement( name, assignId, color, pitch, momentary, activation );
    }

    private static string MapActivation( Articulation articulation )
    {
        var activations = new List<string>();
        var sb = new StringBuilder( 128 );

        activations.AddRange( MapActivationNote( articulation ) );
        activations.AddRange( MapActivationControlChange( articulation ) );
        activations.AddRange( MapActivationProgramChange( articulation ) );

        activations = activations.Distinct().ToList();

        var count = activations.Count;

        for( var i = 0; i < count; i++ )
        {
            var x = activations[ i ];
            sb.Append( x );

            if( i < count - 1 )
            {
                sb.Append( '|' );
            }
        }

        return sb.ToString();
    }

    private static List<string> MapActivationNote( Articulation articulation )
    {
        var result = new List<string>();

        var midiNoteOns = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOn ).ToList();
        var midiNoteOffs = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOff ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in midiNoteOns )
        {
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;
            result.Add( $"note{byte1}.{byte2}" );
        }

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in midiNoteOffs )
        {
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;
            result.Add( $"off{byte1}.{byte2}" );
        }

        return result;
    }

    private static List<string> MapActivationControlChange( Articulation articulation )
    {
        var result = new List<string>();
        var controlChanges = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.ControlChange ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in controlChanges )
        {
            var ccNo = x.Status.Value;
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;

            result.Add(
                ccNo is 0 or 32
                    ? $"bc{byte1}.{byte2}"
                    : $"cc{byte1}.{byte2}"
            );
        }

        return result;
    }

    private static List<string> MapActivationProgramChange( Articulation articulation )
    {
        var result = new List<string>();
        var programChanges = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.ProgramChange ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in programChanges )
        {
            var byte1 = x.Data1.Value;
            result.Add( $"pc{byte1}" );
        }

        return result;
    }
}
