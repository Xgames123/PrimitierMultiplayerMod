using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PrimS
{
	public static class PlayerManager
	{
		public static Dictionary<int, PrimitierPlayer> Players = new Dictionary<int, PrimitierPlayer>();



		public static PrimitierPlayer? GetPlayerById(int id)
		{
			return Players.GetValueOrDefault(id);
		}

		public static void DeletePlayer(int id)
		{
			var player = GetPlayerById(id);
			if (player == null)
				return;

			StorePlayer(new StoredPlayer()
			{
				StaticId = player.StaticId,
				Position = player.HeadPosition,
				Hp = player.Hp,
			});

			Players.Remove(id);
		}


		public static void StorePlayer(StoredPlayer player)
		{	
			World.Settings.Players.Add(player.StaticId, player);
			World.WriteWorldSettings();
		}

		
		public static StoredPlayer? GetStoredPlayer(string staticId)
		{
			return World.Settings.Players.GetValueOrDefault(staticId);
		}


		public static PrimitierPlayer CreateNewPlayer(string username, int runtimeId, string staticId)
		{
			var player = new PrimitierPlayer(username, runtimeId);

			var storedPlayer = GetStoredPlayer(staticId);
			if (storedPlayer != null)
			{
				player.HeadPosition = storedPlayer.Position;
				player.Hp = storedPlayer.Hp;
				player.StaticId = staticId;
			}

			Players.Add(runtimeId, player);
			return player;
		}

	}


}
