using Cocona;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

CoconaLiteApp.Run<App>();

[HasSubCommands(typeof(PrimSConfig.Commands.Reload), "reload", Description = "Reloads the configuration file")]
public class App
{
	

}