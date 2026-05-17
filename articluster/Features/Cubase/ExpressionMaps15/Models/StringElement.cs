using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase.ExpressionMaps15.Models;

[XmlRoot( ElementName = "string" )]
public class StringElement
{
    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute( AttributeName = "value" )]
    public string Value { get; set; } = string.Empty;

    [XmlAttribute( AttributeName = "wide" )]
    public bool Wide { get; set; } = true;

    [XmlIgnore]
    public bool WideSpecified { get; set; } = true;

    public StringElement() {}

    public StringElement( string name, string value, bool wide = true )
    {
        Name  = name;
        Value = value;
        Wide  = wide;
    }
}
