using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XMLElements;

[XmlRoot( ElementName = "InstrumentMap" )]
public class RootElement
{
    [XmlElement( ElementName = "string" )]
    public StringElement Name { get; set; }

    [XmlElement( ElementName = "int" )]
    public IntElement FileVersion { get; set; } = new( "fileVersion", 2 );

    [XmlElement( ElementName = "member" )]
    public List<MemberElement> Members { get; set; } = [ ];

    public RootElement()
    {
        Name = new StringElement( "name", string.Empty );
    }

    public RootElement( string name )
    {
        Name = new StringElement( "name", name );
    }

    public RootElement( string name, IEnumerable<MemberElement> members ) : this( name )
    {
        Members.AddRange( members );
    }
}
