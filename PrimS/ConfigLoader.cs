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

		public DebugConfig? Debugging { get; set; }
	}

	public class DebugConfig
	{
		public bool ShowLocalPlayer { get; set; } = false;
		public bool Debug { get; set; } = false;
	}


	public class ClientConfig
	{
		public int IdleUpdateDelay { get; set; } = 1000;
		public int ActiveUpdateDelay { get; set; } = 20;

	}


	public static class ConfigLoader
	{
		

		private static ILog _log = LogManager.GetLogger(nameof(ConfigLoader));
		private const string c_ConfigFileName = "primsconfig.json";
		private static JsonSerializerOptions? s_options = null;

		public static ConfigFile? Config = null;
		public static event Action<ConfigFile?>? OnConfigReload;

		

		public static bool Load()
		{
			if (s_options == null)
			{
				s_options = new JsonSerializerOptions();
				s_options.ReadCommentHandling = JsonCommentHandling.Skip;
			}
				

			ConfigFile? newConfig;
			try
			{
				newConfig = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText(c_ConfigFileName), s_options);

			}
			catch (Exception e)
			{
				_log.Error("Could not load config", e);
				return false;
			}
			if(newConfig == null)
			{
				_log.Error("loaded config was null");
				return false;
			}

			if(newConfig.Debugging != null && newConfig.Debugging.Debug == false)
			{
				newConfig.Debugging = null;
			}
			


			OnConfigReload?.Invoke(newConfig);
			Config = newConfig;
			return true;
		}


	}

}

