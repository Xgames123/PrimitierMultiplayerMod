using PrimS;
using System;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

public static class Program
{
	public static void Main()
	{
		ConfigLoader.Load();

		World.LoadFromDirectory(ConfigLoader.Config.WorldDirectory);

		//var comunication = new StdInComunication();

		bool stoppingServer = false;
		bool IsServerRunning = true;

		
		var server = new Server();


		Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
		{
			stoppingServer = true;
			server.Stop();
			while (IsServerRunning != false)
			{
				Thread.Sleep(200);
			}

			e.Cancel = true;


		};



		while (true)
		{
			server.Update();
			//comunication.Update();
			if (stoppingServer)
			{
				break;
			}

		}
		IsServerRunning = false;

		//comunication.Dispose();


	}

}

