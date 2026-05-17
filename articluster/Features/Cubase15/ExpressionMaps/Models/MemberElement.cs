using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models;

[XmlRoot( ElementName = "member" )]
public class MemberElement
{
    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; } = string.Empty;

    [XmlElement( ElementName = "int" )]
    public List<IntElement> Int { get; set; } = [ ];

    [XmlElement( ElementName = "float" )]
    public List<FloatElement> Float { get; set; } = [ ];

    [XmlElement( ElementName = "string" )]
    public List<StringElement> String { get; set; } = [ ];

    [XmlElement( ElementName = "obj" )]
    public List<ObjectElement> Obj { get; set; } = [ ];

    [XmlElement( ElementName = "list" )]
    public List<ListElement> List { get; set; } = [ ];

    public MemberElement() {}

    public MemberElement( string name )
    {
        Name = name;
    }
}
