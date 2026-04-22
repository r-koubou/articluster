using System;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal class YamlHexIntConverter : IYamlTypeConverter
{
    public bool Accepts( Type type )
        => type == typeof( int );

    public object ReadYaml( IParser parser, Type type, ObjectDeserializer rootDeserializer )
    {
        var scalar = parser.Consume<Scalar>();

        try
        {
            if( scalar.Value.StartsWith( "0x", StringComparison.OrdinalIgnoreCase ) )
            {
                return Convert.ToInt32( scalar.Value, 16 );
            }

            return int.Parse( scalar.Value );
        }
        catch( Exception ex )
        {
            throw new YamlException( scalar.Start, scalar.End, "Invalid hex or decimal format.", ex );
        }
    }

    public void WriteYaml( IEmitter emitter, object? value, Type type, ObjectSerializer serializer )
    {
        if( value == null )
        {
            return;
        }

        var intValue = (int)value;

        emitter.Emit( new Scalar( null, null, $"0x{intValue:X2}", ScalarStyle.Plain, true, false ) );
    }
}
