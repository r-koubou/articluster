using ArtiCluster.Applications.Cli.Editor;
using ArtiCluster.Applications.Cli.Editor.UI;
using ArtiCluster.Commons.EventEmitting;

using Terminal.Gui.App;

using var app = Application.Create();
app.Init();

var context = new ApplicationContext( new EventEmitter() );
var mainWindow = new MainWindow( context );

app.Run(  mainWindow );
