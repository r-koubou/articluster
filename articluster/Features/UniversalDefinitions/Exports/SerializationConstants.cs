using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitions.Exports;

internal static class SerializationConstants
{
    public static readonly ISerializer DefaultSerializer
        = new SerializerBuilder()
         .ConfigureDefaultValuesHandling( DefaultValuesHandling.Preserve )
         .Build();
}
