using ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XMLElements;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XmlClasses;

public static class USlotVisuals
{
    public static ObjectElement New( string slotName, string description, int articulationType, int group )
    {
        /*
          <obj class="USlotVisuals" ID="105553196598080">
             <int name="displaytype" value="1" />
             <int name="articulationtype" value="1" />
             <string name="text" value="Sustain" wide="true" />
             <int name="symbol" value="0" />
             <int name="smuflSymbol" value="-1" />
             <string name="description" value="Sustain" wide="true" />
             <int name="group" value="0" />
             <int name="userPlayingTechnique" value="0" />
             <string name="techniqueID" value="pt.natural" />
             <string name="ptaID" value="pta.naturaleText" />
             <int name="ptaCategory" value="4" />
           </obj>
        */
        var obj = new ObjectElement( "USlotVisuals" );
        obj.Int.Add( new IntElement( "displaytype", 1 ) );
        obj.Int.Add( new IntElement( "articulationtype", articulationType ) );
        obj.String.Add( new StringElement( "text", slotName ) );
        obj.Int.Add( new IntElement( "symbol", 0 ) );
        obj.Int.Add( new IntElement( "smuflSymbol", -1 ) );
        obj.String.Add( new StringElement( "description", description ) );
        obj.Int.Add( new IntElement( "group", group ) );
        obj.Int.Add( new IntElement( "userPlayingTechnique", 0 ) );
        obj.String.Add( new StringElement( "techniqueID", "pt.natural" ) );
        obj.String.Add( new StringElement( "ptaID", "pta.naturaleText" ) );

        return obj;
    }

    public static ObjectElement New( Articulation articulation, int articulationType, int group )
    {
        return New(
            articulation.Name.Value,
            articulation.Name.Value,
            articulationType,
            group
        );
    }
}
