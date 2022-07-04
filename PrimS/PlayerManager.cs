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

			Players.Remove(player.Id);
		}

		public static PrimitierPlayer CreateNewPlayer(string username, int id)
		{
			var player = new PrimitierPlayer(username, id);

			Players.Add(id, player);
			return player;
		}

	}


}
