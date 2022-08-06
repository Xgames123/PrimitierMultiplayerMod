using LiteNetLib;
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
			{ErrorCode.ModVersionToLow, "The version of the Multilayer mod was to low for the server (To fix this download the latest version from my github)" },
			{ErrorCode.UnsupportedModVersion, "The version of Primitier multilayer mod was not supported by the server"},

		};


		public static void Generate(ref NetDataWriter writer, ref NetPacketProcessor processor, ErrorCode code, string message)
		{
			processor.Write(writer, new ErrorPacket() { ErrorCode = code, Message = message });
		}


		public static void Generate(ref NetDataWriter writer, ref NetPacketProcessor processor, ErrorCode code)
		{
			processor.Write(writer, new ErrorPacket() { ErrorCode = code});
		}


		public static void ReadError(ref NetPacketReader reader, ref NetPacketProcessor processor)
		{
			processor.ReadAllPackets(reader);
		}


		public static string ErrorCodeToString(ErrorCode code)
		{
			if(ErrorMessages.TryGetValue(code, out var result))
			{
				return result;
			}

			return "No error message";
		}
	}
}
