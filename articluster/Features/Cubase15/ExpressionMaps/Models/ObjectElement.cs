using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models;

[XmlRoot( ElementName = "obj" )]
public class ObjectElement
{
    [XmlElement( ElementName = "int" )]
    public List<IntElement> Int { get; set; } = [ ];

    [XmlElement( ElementName = "float" )]
    public List<FloatElement> Float { get; set; } = [ ];

    [XmlElement( ElementName = "string" )]
    public List<StringElement> String { get; set; } = [ ];

    [XmlAttribute( AttributeName = "class" )]
    public string ClassName { get; set; }

    [XmlAttribute( AttributeName = "name" )]
    [MaybeNull]
    public string Name { get; set; } = null!;

    [XmlAttribute( AttributeName = "ID" )]
    public string Id { get; set; }

    [XmlElement( ElementName = "obj" )]
    public List<ObjectElement> Obj { get; set; } = [ ];

    [XmlElement( ElementName = "member" )]
    public List<MemberElement> Member { get; set; } = [ ];

    [SuppressMessage( "ReSharper", "UnusedMember.Global" )]
    public ObjectElement() : this( string.Empty ) {}

    // ReSharper disable once ConvertToPrimaryConstructor
    public ObjectElement( string className )
    {
        Id        = ( BitConverter.ToUInt32( Guid.NewGuid().ToByteArray(), 0 ) & 0x7FFFFFFF ).ToString();
        ClassName = className ?? throw new ArgumentNullException( nameof( className ) );
    }
}
