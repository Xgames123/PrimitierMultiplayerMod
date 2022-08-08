using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;

namespace PrimitierServer
{
	public class ConfigFile
	{
		public string ListenIp { get; set; } = "localhost";
		public int ListenPort { get; set; } = 9543;

		public int MaxPlayers { get; set; } = 10;
		public string WorldDirectory { get; set; } = "World";
		public int UpdateDelay { get; set; } = 10;
		public string? IPCDirectory = null;

		public ClientConfig Client { get; set; }

		
	}

	public class ClientConfig
	{
		public int IdleUpdateDelay { get; set; } = 1000;
		public int ActiveUpdateDelay { get; set; } = 20;

	}


	public static class ConfigLoader
	{
		public static ConfigFile? Config = null;

		private static ILog _log = LogManager.GetLogger(nameof(ConfigLoader));

		public static event Action<ConfigFile?> OnConfigReload;

		private static JsonSerializerOptions? s_options = null;

		public static void Load()
		{
			if (s_options == null)
			{
				s_options = new JsonSerializerOptions();
				s_options.ReadCommentHandling = JsonCommentHandling.Skip;
			}
				

			ConfigFile? newConfig;
			try
			{
				newConfig = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText("primsconfig.json"), s_options);

			}
			catch (Exception e)
			{
				_log.Error("Could not load config", e);
				return;
			}
			if(newConfig == null)
			{
				_log.Error("loaded config was null");
				return;
			}

			OnConfigReload?.Invoke(newConfig);
			Config = newConfig;

		}


	}

}

