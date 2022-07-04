using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.c2s
{
	public class PlayerUpdatePacket
	{
		public Vector3 Position;
		public Vector3 HeadPosition;
		public Vector3 LHandPosition;
		public Vector3 RHandPosition;
	}
}
