using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Exports;

internal static class SerializationConstants
{
    public static readonly ISerializer DefaultSerializer
        = new SerializerBuilder()
         .WithIndentedSequences()
         .WithNewLine( "\n" )
         .ConfigureDefaultValuesHandling( DefaultValuesHandling.OmitNull )
         .Build();
}
