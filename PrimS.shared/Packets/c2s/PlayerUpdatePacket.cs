using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.c2s
{
	public class PlayerUpdatePacket
	{
		public Vector3 Position { get; set; }
		public Vector3 HeadPosition { get; set; }
		public Vector3 LHandPosition { get; set; }
		public Vector3 RHandPosition { get; set; }
	}
}
