using PrimitierServer;
using PrimitierServer.WorldStorage;
using System;
using System.IO;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

public static class Program
{
	public static void Main()
	{
		ConfigLoader.Load();

		World.LoadFromDirectory(ConfigLoader.Config.WorldDirectory);

		var ipcStringListener = new IPCStringListener(
			Path.Combine(ConfigLoader.Config.IPCDirectory, "PRIMITIERSERVER.cmdin"),
			Path.Combine(ConfigLoader.Config.IPCDirectory, "PRIMITIERSERVER.cmdout"));
		ipcStringListener.OnMessage += (string cmd) => 
		{
			return IPCCommandParser.ParseCommand(cmd);
		};

		bool stoppingServer = false;
		bool IsServerRunning = true;

		
		var server = new Server();


		Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
		{
			stoppingServer = true;
			server.Stop();

			e.Cancel = true;


		};



		while (true)
		{
			server.Update();
			if (stoppingServer)
			{
				break;
			}

		}
		IsServerRunning = false;

		ipcStringListener?.Dispose();


	}

}

