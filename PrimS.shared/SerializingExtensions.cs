using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared
{
	public static class SerializingExtensions
	{

		public static void Put(this NetDataWriter writer, Vector3 vector3)
		{
			writer.Put(vector3.X);
			writer.Put(vector3.Y);
			writer.Put(vector3.Z);
		}

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
