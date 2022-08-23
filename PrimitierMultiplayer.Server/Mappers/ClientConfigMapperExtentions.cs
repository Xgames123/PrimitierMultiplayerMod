using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.Mappers
{
	public static class ClientConfigMapperExtentions
	{
		public static NetworkClientConfig ToNetworkClientConfig(this ClientConfig clientConfig)
		{
			return new NetworkClientConfig() { ActiveUpdateDelay = clientConfig.ActiveUpdateDelay, IdleUpdateDelay = clientConfig.IdleUpdateDelay };

		}


	}
}
