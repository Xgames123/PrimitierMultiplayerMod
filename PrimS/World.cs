using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Numerics;

namespace PrimS
{
	public class StoredPlayer
	{
		public Vector3 Position { get; set; }
		public float Hp {get; set; }
		public string StaticId { get; set; }

	}


	public class WorldSettings
	{
		public int Seed { get; set; }

		public Dictionary<string, StoredPlayer> Players { get; set; }

	}

	public static class World
	{
		public const string WorldSettingsPath = "SETTINGS.json";
		public const string ChunkDirectoryPath = "CHUNKS";

		public static string WorldDirectory { get; private set; }

		private static ILog s_log = LogManager.GetLogger(nameof(World));

		public static WorldSettings Settings { get; private set; }

		public static void LoadFromDirectory(string dir)
		{
			WorldDirectory = dir;
			try
			{
				Directory.CreateDirectory(WorldDirectory);
			}
			catch (Exception e)
			{
				s_log.Fatal($"Could not create world directory at location '{WorldDirectory}'", e);
			}

			ReloadWorldSettings();
		}

		public static void WriteWorldSettings()
		{
			var settingsFilePath = Path.Combine(WorldDirectory, WorldSettingsPath);
			try
			{
				File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(Settings));
			}catch(Exception e)
			{
				s_log.Fatal($"Could not write or serialize {WorldSettingsPath}", e);
				return;
			}
			
		}


		public static void ReloadWorldSettings()
		{
			var settingsFilePath = Path.Combine(WorldDirectory, WorldSettingsPath);
			if (File.Exists(settingsFilePath))
			{
				try
				{
					Settings = JsonSerializer.Deserialize<WorldSettings>(File.ReadAllText(settingsFilePath));
				}catch(Exception e)
				{
					s_log.Fatal($"Could not read or deserialize {WorldSettingsPath}", e);
					return;
				}
				
			}
			else
			{
				s_log.Fatal($"No {WorldSettingsPath} file exist in world {WorldDirectory}");
			}

		}



	}
}
