using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase.ExpressionMaps15.Models;

[XmlRoot( ElementName = "list" )]
public class ListElement
{
    [XmlElement( ElementName = "obj" )]
    public List<ObjectElement> Obj { get; set; } = [ ];

    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; }

    [XmlAttribute( AttributeName = "type" )]
    public string Type { get; set; }

    [SuppressMessage( "ReSharper", "UnusedMember.Global" )]
    public ListElement()
    {
        Name = "obj";
        Type = "obj";
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ListElement( string name, string type )
    {
        Name = name;
        Type = type;
    }
}
