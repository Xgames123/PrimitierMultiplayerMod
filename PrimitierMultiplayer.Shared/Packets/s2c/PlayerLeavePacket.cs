﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.Shared.Packets.s2c
{
	public class PlayerLeavePacket : Packet
	{
		public int Id { get; set; }
	}
}