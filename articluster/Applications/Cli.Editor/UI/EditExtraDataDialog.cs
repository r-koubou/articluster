using System.Collections.Generic;
using System.Data;

using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public sealed class EditExtraDataDialog : Dialog
{
    private readonly IApplication inputModalApp;
    private readonly Dictionary<string, string> extraData;
    private readonly DataTable extraEditDataTable = new( "extra" );

    private readonly TableView tableView;

    // ReSharper disable once ConvertToPrimaryConstructor
    public EditExtraDataDialog( Dictionary<string, string> extraData ) : base()
    {
        this.extraData = extraData;
        using var app = Application.Create();
        inputModalApp = app;

        Title  = "Edit extra data";
        Width  = Dim.Percent( 80 );
        Height = Dim.Percent( 60 );

        var closeButton = new Button
        {
            Text = "Close",
        };

        Buttons = [ closeButton ];

        var content = new FrameView()
        {
            X           = 0,
            Y           = 0,
            Width       = Dim.Fill(),
            Height      = Dim.Fill(),
            BorderStyle = LineStyle.None
        };

        tableView = new TableView()
        {
            X        = 0,
            Y        = 0,
            Width    = Dim.Fill(),
            Height   = Dim.Fill(),
            CanFocus = true,
        };

        extraEditDataTable.Columns.Add( "Key" );
        extraEditDataTable.Columns.Add( "Value" );

        foreach( var kvp in extraData )
        {
            extraEditDataTable.Rows.Add( kvp.Key, kvp.Value );
        }

        // for( var i = 0; i < 100; i++ )
        // {
        //     extraEditDataTable.Rows.Add( $"MyExtra.Key{i}", i.ToString() );
        // }

        tableView.ViewportSettings |= ViewportSettingsFlags.HasVerticalScrollBar;
        tableView.Table            =  new DataTableSource( extraEditDataTable );
        tableView.Accepted         += TableViewOnAccepted;

        content.Add( tableView );

        // var listView = new ListView
        // {
        //     X         = 0,
        //     Y         = 0,
        //     Width     = Dim.Fill(),
        //     Height    = Dim.Fill(),
        //     ShowMarks = false,
        //     CanFocus  = true,
        // };
        //
        // listView.ViewportSettings |= ViewportSettingsFlags.HasVerticalScrollBar;
        //
        // //var source = new ObservableCollection<ExtraDataListViewItem>();
        // var source = new ObservableCollection<TextField>();
        //
        // // foreach( var key in extraData.Keys )
        // // {
        // //     source.Add( new ExtraDataListViewItem( key, extraData[ key ] ) );
        // // }
        //
        // for( var i = 0; i < 100; i++ )
        // {
        //     //source.Add( new ExtraDataListViewItem( $"Key {i}", $"Value {i}" ) );
        //     source.Add( new TextField
        //         {
        //             Text = $"item:{i}"
        //         }
        //     );
        // }
        //
        // listView.SetSource( source );
        // content.Add( listView );

        Add( content );

        Accepting += OnAccepting;
    }

    private void OnAccepting( object? sender, CommandEventArgs e )
    {
        extraData.Clear();

        foreach( DataRow row in extraEditDataTable.Rows )
        {
            var key = row[ 0 ].ToString() ?? string.Empty;
            var value = row[ 1 ].ToString() ?? string.Empty;

            extraData[ key ] = value;
        }
    }

    private void UpdateTableView() {}

    private void TableViewOnAccepted( object? sender, CommandEventArgs e )
    {
        if( tableView.Table == null )
        {
            return;
        }

        if( extraEditDataTable.Rows.Count == 0 )
        {
            extraEditDataTable.Rows.Add( "NewKey", "NewValue" );
            return;
        }

        var col = tableView.Value?.Cursor.X ?? 0;
        var row = tableView.Value?.Cursor.Y ?? 0;
        var nowValue = extraEditDataTable.Rows[ row ][ col ].ToString() ?? string.Empty;

        if( !InputTextFieldDialog.Show( tableView.App!, "Edit value", "Value:", nowValue, out var newValue ) )
        {
            return;
        }

        extraEditDataTable.Rows[ row ][ col ] = newValue;
        tableView.Update();
    }
}
