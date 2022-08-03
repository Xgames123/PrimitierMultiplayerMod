using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer.WorldStorage
{
	public class StoredCube
	{
		public uint Id { get; set; }
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public Vector3 Size { get; set; }
		public int Substance { get; set; }
	}
}
