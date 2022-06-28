using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;

namespace PrimS;

public class ConfigFile
{
	public int maxPlayers { get; set; } = 10;

	public string WorldFilePath { get; set; } = "World.primsworld";

	public string ListenIp { get; set; } = "localhost";
	public int ListenPort = 9586;
}


public static class ConfigLoader
{
	public static ConfigFile? Config = null;

	private static ILog _log = LogManager.GetLogger(nameof(ConfigLoader));

	public static event Action<ConfigFile?> OnConfigReload;


	public static void Load()
	{
		ConfigFile newConfig;
		try
		{
			newConfig = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText("primsconfig.json"));

		}
		catch (Exception e)
		{
			_log.Error("Could not load config", e);
			return;
		}
		OnConfigReload?.Invoke(newConfig);
		Config = newConfig;

	}


}
