using PrimitierMultiplayer.Mod;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.ClientLib
{
	public abstract class MultiplayerManagerBase
	{
		public static MultiplayerManagerBase Instance;
		public static IChat Chat;

		public Client Client;
		public int LocalId = -1;
		public bool IsInMultiplayerMode = false;



		public MultiplayerManagerBase(IChat chat)
		{
			Client = new Client();
			Client.OnDisconnectFromServer += Client_OnDisconnectFromServer;
			Client.Start();

			Chat = chat;
		}

		public abstract void ConnectToServer();
		protected abstract void CreateWorld(int seed, System.Numerics.Vector3 playerPosition);
		protected abstract void DestroyWorld();

		protected void ConnectToServer(string address, int port=9543)
		{
			Client.Start();
			Client.Connect(address, port);
		}

		public void EnterGame(int seed, System.Numerics.Vector3 playerPosition)
		{
			IsInMultiplayerMode = true;

			CreateWorld(seed, playerPosition);
		}

		public void ExitGame()
		{
			Chat.Clear();
			IsInMultiplayerMode = false;
			DestroyWorld();
		}


		public void UpdateClient()
		{
			if (Client == null)
				return;
			Client.Update();
		}

		public void Stop()
		{
			ExitGame();

			if (Client.IsRunning)
				Client.Stop();
			
			WorldManager.DestroyAllModChunks();
		}

		private void Client_OnDisconnectFromServer(LiteNetLib.NetPeer obj)
		{
			Stop();
			LocalId = -1;
		}



	}
}
