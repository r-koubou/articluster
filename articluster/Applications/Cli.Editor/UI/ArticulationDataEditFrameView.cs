using System.Collections.Generic;

using ArtiCluster.Applications.Cli.Editor.Model;

using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public sealed class ArticulationDataEditFrameView : FrameView
{
    private readonly ApplicationContext context;
    private readonly ListView<string> listView;

    private ArticulationModel currentModel;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ArticulationDataEditFrameView( ApplicationContext context, ListView<string> listView )
    {
        this.context  = context;
        this.listView = listView;

        currentModel = new ArticulationModel
        {
            Name = "New Articulation",
        };

        Title = "Articulation Edit";

        #region Bind
        listView.ValueChanged += ( _, args ) =>
        {
            if( !TryGetArticulation( args.NewValue, context.Current.Articulations, out var newArticulation ) )
            {
                return;
            }

            SetDataSource( newArticulation );
        };

        if( TryGetArticulation( listView.SelectedItem, context.Current.Articulations, out var articulation ) )
        {
            currentModel = articulation;
        }

        #endregion ~Bind

        #region Setup Components
        UiHelper.CreateTextField( this, "Name", currentModel.Name, 0, newValue =>
            {
                currentModel = currentModel with
                {
                    Name = newValue
                };

                SetNeedsDraw();
            }
        );
        #endregion
    }

    private bool TryGetArticulation( string? name, IEnumerable<ArticulationModel> articulationModels, out ArticulationModel result )
    {
        result = null!;

        if( name == null! )
        {
            return false;
        }

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in context.Current.Articulations )
        {
            if( x.Name != listView.SelectedItem )
            {
                continue;
            }

            result = x;
            return true;
        }

        return false;
    }

    public void SetDataSource( ArticulationModel model )
    {
        currentModel = model;
        SetNeedsDraw();
    }
}
