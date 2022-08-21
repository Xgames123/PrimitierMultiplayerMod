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
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Server.Mappers;

namespace PrimitierMultiplayer.Server.WorldStorage
{


	public class WorldSettings
	{
		public int Seed { get; set; }

		public Vector3 WorldSpawn { get; set; } = Vector3.Zero;

		public Dictionary<string, StoredPlayer> Players { get; set; }
	}

	public static class World
	{
		public const string WorldSettingsPath = "SETTINGS.json";
		public const string ChunkDirectoryPath = "CHUNKS";

		public static string WorldDirectory { get; private set; }

		private static ILog s_log = LogManager.GetLogger(nameof(World));

		public static WorldSettings? Settings { get; private set; }

		private static Dictionary<Vector2, NetworkChunk> Chunks = new Dictionary<Vector2, NetworkChunk>();
		private static Dictionary<Vector2, bool> NeedsSaving = new Dictionary<Vector2, bool>();

		private static JsonSerializerOptions? s_options = null;

		private static void ConfigureJsonOptionsIfNeeded()
		{
			if (s_options == null)
			{
				s_options = new JsonSerializerOptions();
				s_options.Converters.Add(new Vector2Converter());
				s_options.Converters.Add(new Vector3Converter());
				s_options.Converters.Add(new QuaternionConverter());
			}
		}

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
				ConfigureJsonOptionsIfNeeded();
				File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(Settings, s_options));
			}
			catch (Exception e)
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
					Settings = JsonSerializer.Deserialize<WorldSettings>(File.ReadAllText(settingsFilePath), s_options);
				}
				catch (Exception e)
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



		public static void ClearChunkCash()
		{
			SaveAllChunks();
			NeedsSaving.Clear();
			Chunks.Clear();

		}

		public static void SaveAllChunks()
		{
			foreach (var chunk in Chunks.Keys)
			{
				TrySaveChunk(chunk);
			}
		}

		public static bool TrySaveChunk(Vector2 position)
		{
			var chunkName = $"{position.X}_{position.Y}chunk.json";
			if (NeedsSaving.TryGetValue(position, out var needsSaving))
			{
				if (needsSaving)
				{
					try
					{
						var json = JsonSerializer.Serialize(Chunks[position], s_options);
						File.WriteAllText(Path.Combine(WorldDirectory, ChunkDirectoryPath, chunkName), json);
					}
					catch (Exception e)
					{
						s_log.Error($"Could not save chunk '{chunkName}'\nInternalError: {e}");
						return false;
					}

					NeedsSaving[position] = false;
				}

			}
			return true;
		}

		public static void UpdateChunkOwner(Vector2 chunkPosition, int owner)
		{
			if (Chunks.TryGetValue(chunkPosition, out var chunk))
			{
				chunk.Owner = owner;
				Chunks[chunkPosition] = chunk;
				return;
			}

		}

		public static NetworkChunk GetChunk(Vector2 position)
		{
			if (Chunks.TryGetValue(position, out var chunk))
				return chunk;
			var newChunk = LoadChunk(position);
			if (newChunk == null)
				return NetworkChunk.NewBrokenChunk();
			if (newChunk.Cubes.Count == 0)
				return NetworkChunk.NewEmptyChunk();

			var netChunk = newChunk.ToNetworkChunk(-1);
			Chunks.Add(position, netChunk);
			NeedsSaving.Add(position, false);
			return netChunk;
		}
		private static StoredChunk? LoadChunk(Vector2 position)
		{
			string? chunkJson;
			var chunkName = $"{position.X}_{position.Y}chunk.json";
			var chunkPath = Path.Combine(WorldDirectory, ChunkDirectoryPath, chunkName);
			if (!File.Exists(chunkPath))
				return new StoredChunk();

			try
			{
				chunkJson = File.ReadAllText(chunkPath);
			}
			catch (FileNotFoundException e)
			{
				return new StoredChunk();
			}
			catch (DirectoryNotFoundException e)
			{
				return null;
			}
			catch (Exception)
			{
				return null;
			}
			StoredChunk chunk;
			try
			{
				ConfigureJsonOptionsIfNeeded();

				chunk = JsonSerializer.Deserialize<StoredChunk>(chunkJson, s_options);

			}
			catch (Exception e)
			{
				s_log.Error($"Could not parse json for chunk '{chunkName}'\nInternalError: {e}");
				return null;
			}
			return chunk;
		}



	}
}
