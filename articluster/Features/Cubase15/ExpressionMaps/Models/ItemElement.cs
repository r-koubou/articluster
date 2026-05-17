using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models;

[XmlRoot( ElementName = "item" )]
public class ItemElement
{
    [XmlAttribute( AttributeName = "id" )]
    public int Id { get; set; }

    public ItemElement() {}

    public ItemElement( int id )
    {
        Id = id;
    }
}
