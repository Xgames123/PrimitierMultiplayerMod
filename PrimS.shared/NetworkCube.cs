﻿using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared
{
	public struct NetworkCube : INetSerializable
	{
		public uint Id;
		public System.Numerics.Vector3 Position;
		public System.Numerics.Quaternion Rotation;
		public System.Numerics.Vector3 Size;
		public System.Numerics.Vector3 Velosity;
		public System.Numerics.Vector3 AngularVelocity;
		public int Substance;

		public void Serialize(NetDataWriter writer)
		{
			writer.Put(Id);
			writer.Put(Position);
			writer.Put(Rotation);
			writer.Put(Velosity);
			writer.Put(AngularVelocity);
			writer.Put(Size);
			writer.Put(Substance);
		}

		void INetSerializable.Deserialize(NetDataReader reader)
		{
			Id = reader.GetUInt();
			Position = reader.GetVector3();
			Rotation = reader.GetQuaternion();
			Velosity = reader.GetVector3();
			AngularVelocity = reader.GetVector3();
			Size = reader.GetVector3();
			Substance = reader.GetInt();
		}
	}
}
