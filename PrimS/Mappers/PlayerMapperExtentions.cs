using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.Mappers
{
	public static class PlayerMapperExtentions
	{

		public static NetworkPlayer ToNetworkPlayer(this RuntimePlayer primitierPlayer)
		{
			return new NetworkPlayer() { Id = primitierPlayer.RuntimeId, Position = primitierPlayer.Position, HeadPosition = primitierPlayer.HeadPosition, LHandPosition = primitierPlayer.LHandPosition, RHandPosition = primitierPlayer.RHandPosition };
		}

		public static InitialPlayerData ToInitialPlayerData(this RuntimePlayer primitierPlayer)
		{
			return new InitialPlayerData() { Id = primitierPlayer.RuntimeId, Position = primitierPlayer.Position, Username = primitierPlayer.Username };
		}

	}
}
