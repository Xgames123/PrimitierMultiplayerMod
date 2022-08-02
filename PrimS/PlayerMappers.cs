using PrimS.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimS
{
	public static class PlayerMapperExtentions
	{

		public static NetworkPlayer MapToNetworkPlayer(this PrimitierPlayer primitierPlayer)
		{
			return new NetworkPlayer() { Id = primitierPlayer.RuntimeId, Position = primitierPlayer.Position, HeadPosition = primitierPlayer.HeadPosition, LHandPosition = primitierPlayer.LHandPosition, RHandPosition = primitierPlayer.RHandPosition };
		}

	}
}
