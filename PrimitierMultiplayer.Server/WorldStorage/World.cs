using log4net;
using PrimitierMultiplayer.Server.Mappers;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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

		private static Dictionary<Vector2, NetworkChunk> ChunkCache = new Dictionary<Vector2, NetworkChunk>();
		private static List<Vector2> NeedsSaving = new List<Vector2>();

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
			

			LoadWorldSettings();
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


		public static void LoadWorldSettings()
		{
			ConfigureJsonOptionsIfNeeded();
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

		public static void ClearChunkCacheIfMaxSizeExceeded()
		{
			if (ConfigLoader.Config == null)
				return;

			if(ConfigLoader.Config.MaxChunkCacheSize < ChunkCache.Count)
			{
				s_log.Info($"Clearing chunk cache because max size ({ConfigLoader.Config.MaxChunkCacheSize}) is exceeded");
				ClearChunkCache();
			}
			

		}


		public static void ClearChunkCache()
		{
			SaveAllChunks();
			NeedsSaving.Clear();
			ChunkCache.Clear();

		}

		public static void SaveAllChunks()
		{
			foreach (var chunk in ChunkCache.Keys)
			{
				TrySaveChunk(chunk);
			}
		}


		public static void TryOwnChunk(RuntimePlayer player, Vector2 chunkPos, float ownRadius)
		{

			var chunk = GetChunk(chunkPos);

			var playerChunk = ChunkMath.WorldToChunkPos(player.Position);

			if (chunk.Owner == player.RuntimeId)
				return;
			if (chunk.Owner == -1)
			{
				UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}
			var oldPlayer = PlayerManager.GetPlayerById(chunk.Owner);
			if (oldPlayer == null)
			{
				UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}
			if (Vector2.Distance(chunkPos, ChunkMath.WorldToChunkPos(oldPlayer.Position)) >= ownRadius)
			{
				UpdateChunkOwner(playerChunk, player.RuntimeId);
				return;
			}



		}



		public static bool TrySaveChunk(Vector2 position)
		{
			var chunkName = $"{position.X}_{position.Y}chunk.json";
			var nSIndex = NeedsSaving.IndexOf(position);
			if (nSIndex != -1)
			{
				try
				{
					var storedChunk = ChunkCache[position].ToStoredChunk(true);
					if(storedChunk == null)
					{
						s_log.Info($"Corrupted chunk at X: {position.X}, Y: {position.Y}");
						NeedsSaving.RemoveAt(nSIndex);
						return false;
					}
					var json = JsonSerializer.Serialize(storedChunk, s_options);
					File.WriteAllText(Path.Combine(WorldDirectory, ChunkDirectoryPath, chunkName), json);
				}
				catch (Exception e)
				{
					s_log.Error($"Could not save chunk '{chunkName}'\nInternalError: {e}");
					return false;
				}

				NeedsSaving.RemoveAt(nSIndex);
			}

			
			return true;
		}

		private static void UpdateChunkOwner(Vector2 chunkPosition, int owner)
		{
			if (ChunkCache.TryGetValue(chunkPosition, out var chunk))
			{
				chunk.Owner = owner;
				ChunkCache[chunkPosition] = chunk;
				return;
			}

		}

		public static NetworkChunk GetChunk(Vector2 position, bool loadIfDoesntExist=true)
		{
			if (ChunkCache.TryGetValue(position, out var chunk))
				return chunk;
			
			if (loadIfDoesntExist)
			{
				var newChunk = LoadChunk(position);
				var netChunk = NetworkChunk.NewBrokenChunk();
				if (newChunk == null)
				{
					netChunk = NetworkChunk.NewBrokenChunk();
					ChunkCache.Add(position, netChunk);
					return netChunk;
				}
					
				if (newChunk.Cubes.Count == 0)
				{
					netChunk = NetworkChunk.NewEmptyChunk();
					ChunkCache.Add(position, netChunk);
					return netChunk;
				}

				netChunk = newChunk.ToNetworkChunk(-1);
				ChunkCache.Add(position, netChunk);

#if DEBUG
				if (NeedsSaving.Contains(position))
				{
					throw new Exception("Chunk was not fully deleted");
				}
#endif

				return netChunk;
			}
			else
			{
				return NetworkChunk.NewBrokenChunk();
			}
			
		}

		public static bool WriteCube(NetworkCube cube, bool overwrite = true)
		{
			return WriteCube(ChunkMath.WorldToChunkPos(cube.Position), cube, overwrite);
		}

		public static bool WriteCube(Vector2 chunkPos, NetworkCube cube, bool overwrite=true)
		{
			var oldChunk = GetChunk(chunkPos);
			if(oldChunk.ChunkType == NetworkChunkType.Broken)
			{
				return false;
			}
			oldChunk.ChunkType = NetworkChunkType.Normal;
			if (overwrite)
			{
				for (int i = 0; i < oldChunk.Cubes.Count; i++)
				{
					if (oldChunk.Cubes[i].Id == cube.Id)
					{
						oldChunk.Cubes[i] = cube;
						return true;
					}

				}

			}
			oldChunk.Cubes.Add(cube);
			return true;

		}

		public static bool WriteChunk(NetworkChunkPositionPair chunkPosPair)
		{
			return WriteChunk(chunkPosPair.Position, chunkPosPair.Chunk);
		}

		public static bool WriteChunk(Vector2 position, NetworkChunk chunk)
		{
			var oldChunk = GetChunk(position, false);


			if(oldChunk.ChunkType == NetworkChunkType.Broken)
			{
				ChunkCache.Add(position, chunk);
				NeedsSaving.Add(position);
				return false;
			}
			else
			{
				ChunkCache[position] = chunk;
				if(!NeedsSaving.Contains(position))
					NeedsSaving.Add(position);
				return true;
			}

		}

		private static StoredChunk? LoadChunk(Vector2 position)
		{
#if DEBUG
			s_log.Debug($"Loading {position.X}_{position.Y}chunk.json");
#endif

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
