using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Imports;

internal static class DeserializationConstants
{
    public static readonly IDeserializer DefaultDeserializer
        = new DeserializerBuilder()
         .Build();
}
