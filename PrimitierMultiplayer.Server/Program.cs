using log4net;
using PrimitierMultiplayer;
using PrimitierMultiplayer.Server;
using PrimitierMultiplayer.Server.IPC;
using PrimitierMultiplayer.Server.WorldStorage;
using System;
using System.IO;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

public static class Program
{
	private static ILog c_log = LogManager.GetLogger(nameof(Program));

	public static Server Server;

	public static void Main()
	{
		if (!ConfigLoader.Load())
		{
			c_log.Fatal("Can not load configuration file");
			Environment.Exit(-1);
		}
		if (ConfigLoader.Config.Debug != null)
		{
			c_log.Info("Debug mode");
		}
		

		World.LoadFromDirectory(ConfigLoader.Config.WorldDirectory);

		var ipcDir = ConfigLoader.Config.IPCDirectory;
		if(ipcDir == null)
		{
			if(Environment.OSVersion.Platform == PlatformID.Unix)
			{
				ipcDir = "/tmp";
			}else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				ipcDir = Path.GetTempPath();
			}

		}
		
		var ipcStringListener = new IPCStringListener(ipcDir);
		ipcStringListener.OnMessage += (string cmd) => 
		{
			return IPCCommandParser.ParseCommand(cmd);
		};


		bool loopRunning = true;

		
		Server = new Server();
		Server.Start(ConfigLoader.Config);

		Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
		{
			Server.Stop();

			e.Cancel = true;

		};
		ConfigLoader.OnConfigReload += ConfigLoader_OnConfigReload;


		while (true)
		{
			Server.Update();
			World.ClearChunkCacheIfMaxSizeExceeded();
			if (!Server.IsRunning)
			{
				break;
			}

		}
		loopRunning = false;

		ipcStringListener?.Dispose();


	}

	private static void ConfigLoader_OnConfigReload(ConfigFile? config)
	{
		if (config == null)
		{
			c_log.Error("Config reload detected but config was null. Ignoring...");
			return;
		}
		c_log.Info("Config reload detected");
		c_log.Info("Restarting server...");
		Server.Stop();

		Server.Start(config);
	}
}

