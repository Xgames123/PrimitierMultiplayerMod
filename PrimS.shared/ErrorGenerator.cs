using LiteNetLib.Utils;
using PrimitierServer.Shared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierServer.Shared
{
	public static class ErrorGenerator
	{

		public static Dictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>()
		{
			{ErrorCode.Unknown, "Unknown error"},
			{ErrorCode.ServerFull, "The server is full"},
			{ErrorCode.UnsupportedModVersion, "The version of Primitier multilayer mod was not supported by the server"},

		};


		public static void Generate(ref NetDataWriter writer, ref NetPacketProcessor processor, ErrorCode code, string message)
		{
			processor.Write(writer, new ErrorPacket() { ErrorCode = code, Message = message });
		}


		public static void Generate(ref NetDataWriter writer, ref NetPacketProcessor processor, ErrorCode code)
		{
			processor.Write(writer, new ErrorPacket() { ErrorCode = code, Message = ErrorMessages[code]});
		}
	}
}
