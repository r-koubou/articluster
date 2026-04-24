using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Streams;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Tests;

[TestFixture]
public class YamlExportTest
{
    [Test]
    public async Task ExportTestAsync()
    {
        var destPath = Path.GetTempFileName();

        var id = Guid.NewGuid();
        var source = new UniversalDefinition(
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

        try
        {
            await using var stream = File.Create( destPath );
            using var reader = new TextStreamContentWriter( stream );
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
