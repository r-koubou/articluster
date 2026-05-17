using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XMLElements;

[XmlRoot( ElementName = "float" )]
public class FloatElement
{
    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute( AttributeName = "value" )]
    public float Value { get; set; }

    [SuppressMessage( "ReSharper", "UnusedMember.Global" )]
    public FloatElement() {}

    public FloatElement( string name, float value )
    {
        Name  = name;
        Value = value;
    }
}
