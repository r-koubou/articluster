using System;

using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ArtiCluster.Applications.Cli.Editor.UI;

public class GeneralDataEditFrameView : FrameView
{
    private readonly ApplicationContext context;

    // ReSharper disable once ConvertToPrimaryConstructor
    public GeneralDataEditFrameView( ApplicationContext context )
    {
        this.context = context;

        Title    = "General Edit";
        CanFocus = true;

        UiHelper.CreateTextField( this, "Author", context.Current.Author, 0, newValue =>
            {
                context.Current = context.Current with
                {
                    Author = newValue
                };
            }
        );

        UiHelper.CreateTextField( this, "Manufacturer Name", context.Current.ManufacturerName, 1, newValue =>
            {
                context.Current = context.Current with
                {
                    ManufacturerName = newValue
                };
            }
        );

        UiHelper.CreateTextField( this, "Product Name", context.Current.ProductName, 2, newValue =>
            {
                context.Current = context.Current with
                {
                    ProductName = newValue
                };
            }
        );

        UiHelper.CreateTextField( this, "Patch Name", context.Current.PatchName, 3, newValue =>
            {
                context.Current = context.Current with
                {
                    PatchName = newValue
                };
            }
        );

        var editExtraButton = new Button
        {
            Text = "Edit Extra",
            Y = 5
        };

        editExtraButton.Accepting += (s, e) =>
        {
            using var dialog = new EditExtraDataDialog( context.Current.Extra );
            App?.Run( dialog );
        };

        Add( editExtraButton );
    }
}
