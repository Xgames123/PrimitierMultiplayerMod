using log4net;
using PrimitierMultiplayer.Server.WorldStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server
{
	public static class PlayerManager
	{
		public static Dictionary<int, RuntimePlayer> Players = new Dictionary<int, RuntimePlayer>();

		private static ILog s_log = LogManager.GetLogger(nameof(PlayerManager));


		public static RuntimePlayer? GetPlayerById(int id)
		{
			return Players.GetValueOrDefault(id);
		}

		public static IEnumerable<RuntimePlayer> GetAllPlayers()
		{
			return Players.Values;
		}


		public static void DeletePlayer(int id)
		{
			var player = GetPlayerById(id);
			if (player == null)
				return;

			StorePlayer(new StoredPlayer()
			{
				StaticId = player.StaticId,
				Position = player.Position,
				Hp = player.Hp,
			});

			Players.Remove(id);

			if (Players.Count == 0)
			{
				s_log.Info("Clearing chunk cash");
				World.ClearChunkCash();
			}

		}


		public static void StorePlayer(StoredPlayer player)
		{
			World.Settings.Players[player.StaticId] = player;
			World.WriteWorldSettings();
		}


		public static StoredPlayer? GetStoredPlayer(string staticId)
		{
			return World.Settings.Players.GetValueOrDefault(staticId);
		}


		public static RuntimePlayer CreateNewPlayer(string username, int runtimeId, string staticId)
		{
			var player = new RuntimePlayer(username, runtimeId);
			player.StaticId = staticId;
			player.RHandPosition = Vector3.Zero;
			player.LHandPosition = Vector3.Zero;
			player.Hp = 100;
			player.Position = World.Settings.WorldSpawn;

			var storedPlayer = GetStoredPlayer(staticId);
			if (storedPlayer != null)
			{
				player.Position = storedPlayer.Position;
				player.Hp = storedPlayer.Hp;
			}



			Players.Add(runtimeId, player);
			return player;
		}

	}


}
