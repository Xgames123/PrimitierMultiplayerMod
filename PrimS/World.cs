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
using PrimS.shared;

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

		private static Dictionary<Vector2, NetworkChunk> Chunks = new Dictionary<Vector2, NetworkChunk>();

		public static void LoadFromDirectory(string dir)
		{
			WorldDirectory = dir;

			if (!Directory.Exists(dir))
			{
				s_log.Info($"Creating Empty world");
				CreateEmptyWorld();
			}


			ReloadWorldSettings();
		}

		public static NetworkChunk GetChunk(Vector2 position)
		{
			if (Chunks.TryGetValue(position, out var chunk))
			{
				return chunk;
			}
			var newChunk = LoadChunk(position);
			Chunks.Add(position, newChunk);
			return newChunk;
		}
		private static NetworkChunk LoadChunk(Vector2 position)
		{
			string? chunkJson;
			try
			{
				chunkJson = File.ReadAllText(Path.Combine(ChunkDirectoryPath, $"{position.X}_{position.Y}.chunk"));
			}catch(FileNotFoundException e)
			{
				return NetworkChunk.EmptyChunk;
			}catch(DirectoryNotFoundException e)
			{
				return NetworkChunk.EmptyChunk;
			}
			catch(Exception)
			{
				return NetworkChunk.BrokenChunk;
			}
			NetworkChunk chunk;
			try
			{
				chunk = JsonSerializer.Deserialize<NetworkChunk>(chunkJson);

			}catch(Exception)
			{
				return NetworkChunk.BrokenChunk;
			}
			return chunk;
		}


		public static void CreateEmptyWorld()
		{
			try
			{
				Directory.CreateDirectory(WorldDirectory);
			}
			catch (Exception e)
			{
				s_log.Fatal($"Could not create world directory at location '{WorldDirectory}'", e);
			}

			var random = new Random();
			Settings = new WorldSettings()
			{
				Seed = random.Next(int.MinValue, int.MaxValue),
				Players = new Dictionary<string, StoredPlayer>()
			};

			WriteWorldSettings();

			try
			{
				Directory.CreateDirectory(Path.Combine(WorldDirectory, ChunkDirectoryPath));
			}
			catch (Exception e)
			{
				s_log.Fatal($"Could not create {ChunkDirectoryPath} directory at location '{WorldDirectory}'", e);
			}
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
