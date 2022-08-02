using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimS.shared
{
	public struct NetworkCube : INetSerializable
	{
		public uint Id;
		public System.Numerics.Vector3 Position;
		public System.Numerics.Quaternion Rotation;


		public void Serialize(NetDataWriter writer)
		{
			writer.Put(Id);
			writer.Put(Position);
			writer.Put(Rotation);
		}

		void INetSerializable.Deserialize(NetDataReader reader)
		{
			Id = reader.GetUInt();
			Position = reader.GetVector3();
			Rotation = reader.GetQuaternion();
		}
	}
}
