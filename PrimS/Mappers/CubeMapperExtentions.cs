using PrimitierServer.Shared;
using PrimitierServer.WorldStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.Mappers
{
	public static class CubeMapperExtentions
	{
		public static NetworkCube ToNetworkCube(this StoredCube storedCube)
		{
			return new NetworkCube() {Id =  storedCube.Id, Position = storedCube.Position, Rotation = storedCube.Rotation};
		}
	}
}
