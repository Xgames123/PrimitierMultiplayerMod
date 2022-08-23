using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server.WorldStorage
{
	public class StoredPlayer
	{
		public Vector3 Position { get; set; }
		public float Hp { get; set; }
		public string StaticId { get; set; }

	}

}
