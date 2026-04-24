using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal static class SerializationConstants
{
    public static readonly ISerializer DefaultSerializer
        = new SerializerBuilder()
         .ConfigureDefaultValuesHandling( DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitDefaults )
         .Build();

    public static readonly IDeserializer DefaultDeserializer
        = new DeserializerBuilder()
         .Build();
}
