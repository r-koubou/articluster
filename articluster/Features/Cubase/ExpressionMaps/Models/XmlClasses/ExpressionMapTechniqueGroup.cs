using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Models.XmlClasses
{
    /// <summary>
    /// Grouping Articulation Data
    /// </summary>
    /// <remarks>
    /// Added in Cubase 15
    /// </remarks>
    public static class ExpressionMapTechniqueGroup
    {
        public static ObjectElement New( string groupName )
        {
            /*
            <member name="techniqueGroups">
              <list name="obj" type="obj">
                <obj class="ExpressionMapTechniqueGroup" ID="105553392405760">
                  <string name="name" value="HogeGroup1" />
                  <string name="id" value="" />
                </obj>
                <obj class="ExpressionMapTechniqueGroup" ID="105553392416000">
                  <string name="name" value="HogeGroup2" />
                  <string name="id" value="" />
                </obj>
                :
                :
                :
              </list>
            </member>
            */
            var obj = new ObjectElement( "ExpressionMapTechniqueGroup" );
            obj.String.Add( new StringElement( "name", groupName ) );
            obj.String.Add( new StringElement( "value", "" ) );

            return obj;

        }

        public static ObjectElement New( ArticulationGroup articulationGroup )
        {
            return New( articulationGroup.Name.Value );
        }
    }
}
