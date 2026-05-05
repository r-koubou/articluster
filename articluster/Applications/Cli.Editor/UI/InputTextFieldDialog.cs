using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public static class InputTextFieldDialog
{
    public static bool Show( IApplication app, string title, string label, string initialValue, out string result )
    {
        var okClicked = false;

        var dialog = new Dialog
        {
            Title = title,
        };

        var labelView = new Label
        {
            X    = 0,
            Y    = 0,
            Text = label,
        };

        var textField = new TextField
        {
            X     = 0,
            Y     = 1,
            Width = Dim.Fill(),
            Text  = initialValue,
        };

        var okButton = new Button
        {
            Text      = "OK",
            IsDefault = true,
        };

        okButton.Accepting += ( _, _ ) =>
        {
            okClicked = true;
            dialog.App?.RequestStop();
        };

        var cancelButton = new Button
        {
            Text = "Cancel",
        };

        cancelButton.Accepting += ( _, _ ) =>
        {
            dialog.App?.RequestStop();
        };

        dialog.Add( labelView, textField );
        dialog.Buttons   = [ okButton, cancelButton ];
        textField.SetFocus();

        app.Run( dialog );
        dialog.Dispose();

        result = okClicked ? textField.Text : string.Empty;

        return okClicked;
    }
}
