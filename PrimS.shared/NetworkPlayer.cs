using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PrimitierServer.shared
{
	public struct NetworkPlayer : INetSerializable
	{
		public int Id;
		public Vector3 Position;
		public Vector3 HeadPosition;
		public Vector3 LHandPosition;
		public Vector3 RHandPosition;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(Id);
			writer.Put(Position);
			writer.Put(HeadPosition);
			writer.Put(LHandPosition);
			writer.Put(RHandPosition);
		}

		public void Deserialize(NetDataReader reader)
		{
			Id = reader.GetInt();
			Position = reader.GetVector3();
			HeadPosition = reader.GetVector3();
			LHandPosition = reader.GetVector3();
			RHandPosition = reader.GetVector3();

		}
	}
}
