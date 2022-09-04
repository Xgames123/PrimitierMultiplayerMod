using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LiteNetLib;
using log4net;

namespace PrimitierMultiplayer.Server
{
	public class ConfigFile
	{
		public string ListenIp { get; set; } = "localhost";
		public int ListenPort { get; set; } = 9543;
		public string? IPCDirectory = null;

		public int MaxPlayers { get; set; } = 10;
		public int UpdateDelay { get; set; } = 10;
		

		public ClientConfig Client { get; set; } = new ClientConfig();


		public string WorldDirectory { get; set; } = "World";
		public int MaxChunkCacheSize { get; set; } = 4_000;
		public int ViewRadius { get; set; } = 1;

		public DebugConfig? Debug { get; set; }
	}

	public class DebugConfig
	{
		public bool ShowLocalPlayer { get; set; } = false;
		public bool Debug { get; set; } = false;
		public bool ShowChunkBounds { get; set; } = false;
	}


	public class ClientConfig
	{
		public int IdleUpdateDelay { get; set; } = 1000;
		public int ActiveUpdateDelay { get; set; } = 20;

	}


	public static class ConfigLoader
	{


		private static ILog c_log = LogManager.GetLogger(nameof(ConfigLoader));
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
				c_log.Error("Could not load config", e);
				return false;
			}
			if (newConfig == null)
			{
				c_log.Error("loaded config was null");
				return false;
			}

			newConfig = ValidateConfig(newConfig);


			OnConfigReload?.Invoke(newConfig);
			Config = newConfig;
			return true;
		}

		private static ConfigFile ValidateConfig(ConfigFile config)
		{
			if (config.Debug != null && config.Debug.Debug == false)
				config.Debug = null;
			if (config.UpdateDelay < 50)
			{
				c_log.Warn("UpdateDelay was smaller than 50 ms. The client can not handle so many packets");
				config.UpdateDelay = 50;
			}

			if(config.ViewRadius <= 0)
			{
				if (config.Debug == null)
				{
					c_log.Error("ViewRadius can not be less than or equal to 0");
					config.ViewRadius = 1;
				}
				else
				{
					c_log.Warn("ViewRadius is less than or equal to 0 (This is only allowed in debug mode)");
				}
					
			}


			
			return config;
		}

	}

}

