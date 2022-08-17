using PrimitierServer.Shared;
using PrimitierServer.Shared.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.Mappers
{
	public static class ClientConfigMapperExtentions
	{
		public static NetworkClientConfig ToNetworkClientConfig(this ClientConfig clientConfig)
		{
			return new NetworkClientConfig() { ActiveUpdateDelay = clientConfig.ActiveUpdateDelay, IdleUpdateDelay = clientConfig.IdleUpdateDelay};

		}


	}
}
