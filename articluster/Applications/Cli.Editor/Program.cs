using ArtiCluster.Applications.Cli.Editor.UI;

using Terminal.Gui.App;

using var app = Application.Create();
app.Init();

var mainWindow = new MainWindow( app );
app.Run(  mainWindow );
