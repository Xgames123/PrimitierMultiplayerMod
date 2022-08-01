using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public class NetworkCube
	{
		public uint Id;
		public System.Numerics.Vector3 Position;
		public System.Numerics.Quaternion Rotation;

		public static NetworkCube Deserialize(NetDataReader reader)
		{
			return new NetworkCube() { Id = reader.GetUInt(), Position = reader.GetVector3(), Rotation = reader.GetQuaternion() };
		}

		public static void Serialize(NetDataWriter writer, NetworkCube cube)
		{
			writer.Put(cube.Id);
			writer.Put(cube.Position);
			writer.Put(cube.Rotation);
		}
	}
}
