﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PrimS.shared.Packets.s2c
{
	public class PlayerJoinedPacket
	{
		public int Id;
		public string Username;
		public Vector3 Position;

	}
}
