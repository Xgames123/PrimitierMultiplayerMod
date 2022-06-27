using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PrimS;
public static class PlayerManager
{
	public static Dictionary<int, PrimitierPlayer> Players = new Dictionary<int, PrimitierPlayer>();

	public static Dictionary<IPEndPoint, int> IPToIdDict = new Dictionary<IPEndPoint, int>();

	public static int GenerateNewId()
	{
		while (true)
		{
			var value = Random.Shared.Next(int.MinValue, int.MaxValue);
			if (!Players.ContainsKey(value))
				return value;

		}
		
	}

	public static PrimitierPlayer? GetPlayerById(int id)
	{
		return Players.GetValueOrDefault(id);
	}
	public static PrimitierPlayer? GetPlayerByIpAddress(IPEndPoint endpoint)
	{
		return GetPlayerById(IPToIdDict.GetValueOrDefault(endpoint));
	}

	public static void DeletePlayer(int id)
	{
		var player = GetPlayerById(id);
		if (player == null)
			return;

		Players.Remove(player.Id);
		IPToIdDict.Remove(player.Endpoint);
	}

	public static PrimitierPlayer CreateNewPlayer(string username, IPEndPoint endpoint)
	{
		var id = GenerateNewId();
		var player = new PrimitierPlayer(username, id, endpoint);

		Players.Add(id, player);
		IPToIdDict.Add(endpoint, id);

		return player;
	}
	
}
