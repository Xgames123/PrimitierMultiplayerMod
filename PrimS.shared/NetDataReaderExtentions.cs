using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared
{
	public static class NetDataReaderExtentions
	{

		public static Vector3 GetVector3(this NetDataReader reader)
		{
			var vector = new Vector3();

			vector.X =reader.GetFloat();
			vector.Y = reader.GetFloat();
			vector.Z = reader.GetFloat();

			return vector;
		}
	}
}
