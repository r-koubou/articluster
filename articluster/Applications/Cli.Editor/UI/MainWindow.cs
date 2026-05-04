using Terminal.Gui.Configuration;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public class MainWindow : Window
{
    private readonly ApplicationContext context;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MainWindow( ApplicationContext context )
    {
        this.context = context;
        InitComponent();
    }

    private void InitComponent()
    {
        Title      = "ArtiCluster Editor";
        Width      = Dim.Fill();
        Height     = Dim.Fill();
        SchemeName = SchemeManager.SchemesToSchemeName( Schemes.Accent );

        var leftFrame = new ArticulationListFrameView( context )
        {
            X           = 0,
            Y           = 0,
            Width       = Dim.Percent( 30 ),
            Height      = Dim.Fill(),
            BorderStyle = LineStyle.Single,
            CanFocus    = true
        };

        var rightFrame = new FrameView
        {
            X           = Pos.Right( leftFrame ),
            Width       = Dim.Fill(),
            Height      = Dim.Fill(),
            BorderStyle = LineStyle.None,
            CanFocus    = true
        };

        var rightTopFrame = new GeneralDataEditFrameView( context )
        {
            Width       = Dim.Fill(),
            Height      = Dim.Percent( 40 ),
            BorderStyle = LineStyle.Single,
            CanFocus    = true
        };

        var rightBottomFrame = new ArticulationDataEditFrameView( context, leftFrame.ListView )
        {
            Y           = Pos.Bottom( rightTopFrame ),
            Width       = Dim.Fill(),
            Height      = Dim.Fill(),
            BorderStyle = LineStyle.Single,
            CanFocus    = true
        };

        rightFrame.Add( rightTopFrame, rightBottomFrame );


        Add( leftFrame, rightFrame );
    }
}
