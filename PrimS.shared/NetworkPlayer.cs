using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PrimS.shared
{
	public class NetworkPlayer
	{
		public int Id;
		public Vector3 Position;
		public Vector3 HeadPosition;
		public Vector3 LHandPosition;
		public Vector3 RHandPosition;

		public static NetworkPlayer Deserialize(NetDataReader reader)
		{
			return new NetworkPlayer()
			{
				Id = reader.GetInt(),
				Position = reader.GetVector3(),
				HeadPosition = reader.GetVector3(),
				LHandPosition = reader.GetVector3(),
				RHandPosition = reader.GetVector3(),
			};

			
			
		}

		public static void Serialize(NetDataWriter writer, NetworkPlayer player)
		{
			writer.Put(player.Id);
			writer.Put(player.Position);
			writer.Put(player.HeadPosition);
			writer.Put(player.LHandPosition);
			writer.Put(player.RHandPosition);

		}
	}
}
