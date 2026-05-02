using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitionsNew.Imports;

internal static class DeserializationConstants
{
    public static readonly IDeserializer DefaultDeserializer
        = new DeserializerBuilder()
         .Build();
}
