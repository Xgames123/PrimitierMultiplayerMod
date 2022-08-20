using PrimitierMultiplayerMod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public class RuntimeChunk
	{
		public int Owner;

		public List<uint> NetworkSyncs = new List<uint>();

	}
}
