﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.s2c
{
	public class PlayerJoinedPacket
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public Vector3 Position { get; set; }

	}
}