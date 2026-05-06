using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Models;

[XmlRoot( ElementName = "Music.KeySwitchList" )]
public class StudioOneRootElement
{
    [XmlElement( ElementName = "Attributes" )]
    public List<ElementAttribute> AttributeElements { get; set; } = [ ];

    [XmlAttribute( AttributeName = "name" )]
    public string Name { get; set; } = string.Empty;
}
