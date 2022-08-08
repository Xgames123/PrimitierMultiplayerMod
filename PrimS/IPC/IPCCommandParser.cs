using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;
using PrimitierServer.Shared;
using PrimitierServer.WorldStorage;

namespace PrimitierServer.IPC
{



	public static class IPCCommandParser
	{
		private static ILog s_log = LogManager.GetLogger(nameof(IPCCommandParser));

		public static string? ParseCommand(string str)
		{
			var options = new JsonSerializerOptions()
			{
				Converters ={
					new JsonStringEnumConverter()
				}

			};

			try
			{
				var cmd = JsonSerializer.Deserialize<IPCCommand>(str, options);

				var responce = RunCmd(cmd);
				return JsonSerializer.Serialize(responce, options);

			}
			catch (JsonException)
			{
				return JsonSerializer.Serialize(IPCResponce.InvalidCommand(), options);
			}


		}


		private static IPCResponce? RunCmd(IPCCommand? cmd)
		{
			if (cmd == null)
				return null;

			switch (cmd.Type)
			{
				case IPCCommandType.RELOAD_CONF:
					ConfigLoader.Load();
					return IPCResponce.Ok();

				case IPCCommandType.LS_PLAYERS:
					return IPCResponce.Ok(PlayerManager.GetAllPlayers());

				case IPCCommandType.RELOAD_WORLD:
					World.ReloadWorldSettings();
					World.ClearChunkCash();
					return IPCResponce.Ok();

				default:
					s_log.ErrorFormat("IPC command '{command}' not implemented", cmd.Type);
					return IPCResponce.InvalidCommand();
			}
		}

	}
}
