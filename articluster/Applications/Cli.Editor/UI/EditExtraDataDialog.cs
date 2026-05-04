using System.Collections.Generic;

using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public sealed class EditExtraDataDialog : Dialog
{
    private readonly Dictionary<string, string> extraData;

    // ReSharper disable once ConvertToPrimaryConstructor
    public EditExtraDataDialog( Dictionary<string, string> extraData ) : base()
    {
        this.extraData = extraData;

        Title  = "Edit extra data";
        Width  = Dim.Percent( 80 );
        Height = Dim.Percent( 60 );

        var closeButton = new Button
        {
            Text = "Close",
        };

        Buttons = [ closeButton ];

        var scrollBar = new ScrollBar()
        {
            X                     = Pos.AnchorEnd (),
            VisibilityMode        = ScrollBarVisibilityMode.Manual,
            Visible               = true,
            ScrollableContentSize = 100,
            Height                = Dim.Fill ()
        };

        var content = new FrameView()
        {
            X           = 0,
            Y           = 0,
            Width       = Dim.Fill(),
            Height      = Dim.Fill(),
            BorderStyle = LineStyle.None
        };

        content.ViewportSettings |= ViewportSettingsFlags.HasVerticalScrollBar;

        var listView = new ListView
        {
            X = 0,
            Y = 0,
            Width  = Dim.Fill(),
            Height = Dim.Fill(),
        };

        //var source = new ObservableCollection<Dictionary<string, string>>( extraData );

        content.Add( scrollBar );

        for( var i = 0; i < 100; i++ )
        {
            var label = new Label
            {
                Text = i.ToString(),
                Y = i
            };
            content.Add( label );
        }

        Add( content );
    }
}
