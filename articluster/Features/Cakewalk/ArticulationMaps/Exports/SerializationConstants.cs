using System.Text.Json;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Exports;

internal static class SerializationConstants
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        IndentSize    = 2,
        NewLine       = "\n"
    };
}
