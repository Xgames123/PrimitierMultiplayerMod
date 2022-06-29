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
			return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
		}
	}
}
