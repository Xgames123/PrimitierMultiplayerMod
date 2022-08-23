using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimitierMultiplayer.Shared
{
	public static class SystemNumericsSerializingExtensions
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

		public static void Put(this NetDataWriter writer, Vector2 vector2)
		{
			writer.Put(vector2.X);
			writer.Put(vector2.Y);
		}

		public static Vector2 GetVector2(this NetDataReader reader)
		{
			return new Vector2(reader.GetFloat(), reader.GetFloat());
		}


		public static void PutList<T>(this NetDataWriter writer, List<T> list) where T : INetSerializable, new()
		{
			writer.Put(list.Count);

			foreach (var item in list)
			{
				writer.Put(item);
			}

		}
		public static List<T> GetList<T>(this NetDataReader reader) where T : INetSerializable, new()
		{
			var count = reader.GetInt();
			var list = new List<T>(count);

			for (int i = 0; i < count; i++)
			{
				list.Add(reader.Get<T>());
			}

			return list;
		}


		public static void Put(this NetDataWriter writer, Quaternion quaternion)
		{
			writer.Put(quaternion.X);
			writer.Put(quaternion.Y);
			writer.Put(quaternion.Z);
			writer.Put(quaternion.W);
		}

		public static Quaternion GetQuaternion(this NetDataReader reader)
		{
			return new Quaternion(reader.GetFloat(), reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
		}
	}
}
