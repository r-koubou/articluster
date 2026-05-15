using System;
using System.Collections.Generic;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Shared.Mock;

public static class MockUniversalDefinition
{
    public static UniversalDefinitionProductCollection CreateCollection()
    {
        return new UniversalDefinitionProductCollection(
            [
                CreateDefinition( Guid.NewGuid(), manufacturerName: "Acme Corp", productName: "Super Synth", patchName: "Epic Lead" ),
                CreateDefinition( Guid.NewGuid(), manufacturerName: "Acme Corp X", productName: "Great Synth", patchName: "Epic Lead" ),
                CreateDefinition( Guid.NewGuid(), manufacturerName: "Acme Corp Y", productName: "Better Synth", patchName: "Epic Lead" ),
                CreateDefinition( Guid.NewGuid(), manufacturerName: "Acme Corp Y", productName: "Goog Synth", patchName: "Epic Lead" ),
            ]
        );
    }

    public static UniversalDefinitionProductSet CreateProductSet(
        string manufacturerName = "Acme Corp",
        string productName = "Super Synth" )
    {
        return UniversalDefinitionProductSet.Create(
            manufacturerName: manufacturerName,
            productName: productName,
            items:
            [
                CreateDefinition( Guid.NewGuid(), manufacturerName, productName ),
                CreateDefinition( Guid.NewGuid(), manufacturerName, productName, patchName: "Warm Pad" )
            ]
        );
    }

    public static UniversalDefinition CreateDefinition(
        Guid id,
        string manufacturerName = "Acme Corp",
        string productName = "Super Synth",
        string patchName = "Epic Lead" )
    {
        return UniversalDefinition.Create(
            id: id,
            author: "John Doe",
            manufacturerName: manufacturerName,
            productName: productName,
            patchName: patchName,
            description: "multi-line\ndescription",
            extra: new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
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
            ]
        );
    }
}
