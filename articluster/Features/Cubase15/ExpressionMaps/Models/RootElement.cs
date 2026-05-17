using System.Collections.Generic;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models;

[XmlRoot( ElementName = "InstrumentMap" )]
public class RootElement
{
    [XmlElement( ElementName = "string" )]
    public StringElement StringElement { get; set; }

    [XmlElement( ElementName = "member" )]
    public List<MemberElement> Member { get; set; } = [ ];

    public RootElement()
    {
        StringElement = new StringElement( "name", string.Empty );
    }

    public RootElement( string name )
    {
        StringElement = new StringElement( "name", name );
    }

    public RootElement( string name, IEnumerable<MemberElement> members ) : this( name )
    {
        Member.AddRange( members );
    }
}
