using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Models;

public class ElementAttribute
{
    public const int NoPitch = -1;

    [XmlAttribute( AttributeName = "folder" )]
    public string? Folder { get; set; }

    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute( AttributeName = "id" )]
    public string Id { get; set; }  = string.Empty;

    [XmlAttribute( AttributeName = "color" )]
    public string? Color { get; set; } // AABBGGRR

    [XmlAttribute( AttributeName = "pitch" )]
    public string? Pitch { get; set; }

    [XmlAttribute( AttributeName = "momentary" )]
    public string Momentary { get; set; } = string.Empty;

    [XmlAttribute( AttributeName = "activation" )]
    public string Activation { get; set; }  = string.Empty;

    [XmlElement( ElementName = "Attributes" )]
    public List<ElementAttribute> Children { get; } = new();

    public ElementAttribute() {}

    public ElementAttribute(
        string name,
        int id,
        string? color,
        int pitch,
        int momentary,
        string activation )
    {
        Name       = name;
        Id         = id.ToString();
        Color      = color;
        Pitch      = pitch != NoPitch ? pitch.ToString() : null;
        Momentary  = momentary.ToString();
        Activation = activation;
    }

    public ElementAttribute(
        string name,
        int id,
        string color,
        int momentary,
        string activation
    ) : this( name, id, color, NoPitch, momentary, activation ) {}
}
