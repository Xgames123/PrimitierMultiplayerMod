using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;
using PrimitierMultiplayer.Server;
using PrimitierMultiplayer.Server.WorldStorage;
using PrimitierMultiplayer.Shared;

namespace PrimitierMultiplayer.Server.IPC
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

			s_log.InfoFormat("Got IPC Command {0}", cmd.Type.ToString());
			

			switch (cmd.Type)
			{
				case IPCCommandType.Invalid:
					return IPCResponce.InvalidCommand();

				case IPCCommandType.ReloadConfig:
					s_log.Info("Reloading config...");
					if (ConfigLoader.Load())
						return IPCResponce.Ok();
					else
						return IPCResponce.Error("Could not load config file");

				case IPCCommandType.ListPlayers:
					s_log.Info("Listing players...");
					return IPCResponce.Ok(PlayerManager.GetAllPlayers());

				case IPCCommandType.ReloadWorld:
					s_log.Info("Reloading world...");
					World.LoadWorldSettings();
					World.ClearChunkCache();
					return IPCResponce.Ok();

				case IPCCommandType.SaveWorld:
					s_log.Info("Saving world...");
					World.SaveAllChunks();
					return IPCResponce.Ok();

				default:
					s_log.ErrorFormat("IPC command '{command}' not implemented", cmd.Type);
					return IPCResponce.InvalidCommand();
			}
		}

	}
}
