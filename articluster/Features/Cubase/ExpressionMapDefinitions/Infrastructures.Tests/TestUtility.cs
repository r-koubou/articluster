using System;
using System.Collections.Generic;
using System.IO;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using NUnit.Framework;

namespace ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures.Tests;

internal static class TestUtility
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly string TestDataDirectoryRoot = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );

    public static string MakeTestDataPath( string relativePath )
        => Path.Combine( TestDataDirectoryRoot, relativePath );

    public static UniversalDefinition CreateMock(
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
