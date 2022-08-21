using PrimitierMultiplayer.Mod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Mod
{
	public class RuntimeChunk
	{
		public int Owner;

		public List<uint> NetworkSyncs = new List<uint>();

	}
}
