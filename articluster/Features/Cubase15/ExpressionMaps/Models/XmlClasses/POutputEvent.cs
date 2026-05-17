using ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XMLElements;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XmlClasses;

public static class POutputEvent
{
    public static ObjectElement New( int midiStatus, int? data1 = null, int? data2 = null )
    {
        /*
        <obj class="POutputEvent" ID="4196276652">
           <int name="status" value="176"/>
           <int name="data1" value="1"/>
           <int name="data2" value="23"/>
        </obj>
        */
        var obj = new ObjectElement( "POutputEvent" );

        obj.Int.Add( new IntElement( "status", midiStatus ) );

        if( data1 != null )
        {
            obj.Int.Add( new IntElement( "data1", data1.Value ) );
        }

        if( data2 != null )
        {
            obj.Int.Add( new IntElement( "data2", data2.Value ) );
        }

        return obj;
    }

}
