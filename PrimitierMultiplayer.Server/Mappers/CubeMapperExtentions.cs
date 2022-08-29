using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.Mappers
{
	public static class CubeMapperExtentions
	{
		public static NetworkCube ToNetworkCube(this StoredCube storedCube)
		{
			return new NetworkCube() { Id = storedCube.Id, Position = storedCube.Position, Rotation = storedCube.Rotation, Size = storedCube.Size, Substance = storedCube.Substance };
		}

		public static StoredCube ToStoredCube(this NetworkCube networkCube)
		{
			return new StoredCube() { Id = networkCube.Id, Position = networkCube.Position, Rotation = networkCube.Rotation, Size = networkCube.Size, Substance = networkCube.Substance };
		}
	}
}
