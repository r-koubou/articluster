using System;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services;

public class UniversalDefinitionService
{
    public static UniversalDefinition CreateTemplate()
        => UniversalDefinition.Create(
            id: Guid.NewGuid(),
            author: "Example Author",
            manufacturerName: "Example Manufacturer",
            productName: "Example Product",
            patchName: "Example Patch",
            description: "Example Description",
            articulations:
            [
                Articulation.Create(
                    name: "Articulation Name",
                    midiMessages:
                    [
                        MidiMessage.Create( 0x90, 60, 100 )
                    ]
                )
            ]
        );
}
