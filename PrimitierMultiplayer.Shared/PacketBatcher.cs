using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared
{
	public class CubeUpdateBatcher
	{
		private List<NetworkCube> _list = new List<NetworkCube>();


		public void Add(NetworkCube networkCube)
		{

		}

		public IEnumerable<Packet> Flush()
		{


		}

	}
}
