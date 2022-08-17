using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimitierServer.Shared;

namespace PrimitierServer.Mappers
{
	public static class DebugConfigMapperExtentions
	{

		public static NetworkDebugConfig ToNetworkDebugConfig(this DebugConfig debugConfig)
		{
			return new NetworkDebugConfig() { ShowLocalPlayer = debugConfig.ShowLocalPlayer };
		}

	}
}
