using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Features.UniversalDefinitions.Exports;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Streams;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Tests;

[TestFixture]
public class YamlExportTest
{
    [Test]
    public async Task ExportTestAsync()
    {
        var destPath = Path.GetTempFileName();

        var id = Guid.NewGuid();
        var source = UniversalDefinition.Create(
            id: id,
            author: "John Doe",
            manufacturerName: "Acme Corp",
            productName: "Super Synth",
            patchName: "Epic Lead",
            description: "multi-line\ndescription",
            extra: new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
            articulationGroups:
            [
                ArticulationGroup.Create(
                    name: "Main",
                    articulations:
                    [
                        Articulation.Create(
                            name: "Sustain",
                            midiMessages:
                            [
                                // Note On
                                MidiMessage.Create( 0x90, 40, 100 ),
                                // Note Off
                                MidiMessage.Create( 0x80, 40, 110 ),
                                // Control Change
                                MidiMessage.Create( 0xB0, 1, 127 ),
                                // Program Change
                                MidiMessage.Create( 0xC0, 49 ),
                            ],
                            extra: new Dictionary<string, string>
                            {
                                { "LocalKey", "LocalValue" }
                            }
                        )
                    ],
                    extra: new Dictionary<string, string>
                    {
                        { "GroupKey", "GroupValue" }
                    }
                )
            ]
        );

        try
        {
            await using var stream = File.Create( destPath );
            await using var reader = new TextStreamContentWriter( stream );
            var exporter = new YamlExporter();
            var result = await exporter.ExportAsync( reader, source, CancellationToken.None );

            Assert.That( result.IsSuccess, Is.True, "Export should succeed" );
        }
        finally
        {
            if( File.Exists( destPath ) )
            {
                var text = await File.ReadAllTextAsync( destPath );
                await TestContext.Out.WriteLineAsync( "#### Exported YAML content:" );
                await TestContext.Out.WriteAsync( text );
                File.Delete( destPath );
            }
        }
    }
}
