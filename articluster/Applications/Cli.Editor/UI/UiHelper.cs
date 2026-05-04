using System;

using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public static class UiHelper
{
    public static void CreateTextField( View target, string title, string value, int y, Action<string> valueChanged )
    {
        var label = new Label
        {
            Text     = title,
            X        = 1,
            Y        = y,
            CanFocus = false
        };

        var textField = new TextField()
        {
            X     = 20,
            Y     = Pos.Top( label ),
            Width = Dim.Fill() - 2,
            Value = value
        };

        textField.ValueChanged += ( _, args ) =>
        {
            valueChanged( args.NewValue ?? string.Empty );
        };

        target.Add( label, textField );
    }
}
