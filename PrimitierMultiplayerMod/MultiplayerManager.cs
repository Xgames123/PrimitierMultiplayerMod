using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class MultiplayerManager
	{
		public static Client Client;
		public static MelonPreferences_Entry<string> ServerAddress = null;
		public static MelonPreferences_Entry<int> ServerPort = null;


		public static void ConnectToServer()
		{

			var connectSettings = MelonPreferences.CreateCategory("ConnectSettings");

			if(ServerAddress == null)
				ServerAddress = connectSettings.CreateEntry("ServerIp", "localhost");
			if (ServerPort == null)
				ServerPort = connectSettings.CreateEntry<int>("ServerPort", 9543);


			Client = new Client();
			Client.Connect(ServerAddress.Value, ServerPort.Value);
		}

		public static void UpdateClient()
		{
			if (Client == null)
				return;
			Client.Update();
		}

		public static void Stop()
		{
			if (Client == null)
				return;

			Client.Stop();
		}
	}
}
