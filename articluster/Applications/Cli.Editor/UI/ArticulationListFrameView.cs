using System.Collections.ObjectModel;

using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public sealed class ArticulationListFrameView : FrameView
{
    private readonly ApplicationContext context;
    private readonly ListView<string> listView;

    public ListView<string> ListView
        => listView;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ArticulationListFrameView( ApplicationContext context )
    {
        this.context = context;

        Title = "Articulations";

        var items = new ObservableCollection<string>();

        foreach( var x in context.Current.Articulations )
        {
            items.Add( x.Name );
        }

        var listView = new ListView<string>
        {
            X         = 0,
            Y         = 0,
            Width     = Dim.Fill(),
            Height    = Dim.Fill(),
            ShowMarks = false,
            CanFocus  = true,
        };
        listView.ViewportSettings |= ViewportSettingsFlags.HasVerticalScrollBar;
        listView.SetSource( items );

        Add( listView );

        this.listView = listView;
    }
}
