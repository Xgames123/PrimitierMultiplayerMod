using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models.Config;

namespace PrimitierMultiplayer.Server.Mappers
{
	public static class DebugConfigMapperExtentions
	{

		public static NetworkDebugConfig ToNetworkDebugConfig(this DebugConfig debugConfig)
		{
			return new NetworkDebugConfig() { ShowLocalPlayer = debugConfig.ShowLocalPlayer, ShowChunkBounds = debugConfig.ShowChunkBounds };
		}

	}
}
