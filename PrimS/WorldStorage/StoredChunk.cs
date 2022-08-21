using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.WorldStorage
{
	public class StoredChunk
	{
		public List<StoredCube> Cubes { get; set; } = new List<StoredCube>();

	}
}
