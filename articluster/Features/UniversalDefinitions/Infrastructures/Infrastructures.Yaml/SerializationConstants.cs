using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;

internal static class SerializationConstants
{
    public static readonly ISerializer DefaultSerializer
        = new SerializerBuilder()
         .ConfigureDefaultValuesHandling( DefaultValuesHandling.Preserve )
         .Build();

    public static readonly IDeserializer DefaultDeserializer
        = new DeserializerBuilder()
         .Build();
}
