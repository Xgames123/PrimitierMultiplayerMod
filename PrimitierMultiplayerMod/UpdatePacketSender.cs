using PrimitierModdingFramework;
using PrimitierServer.shared.Packets.c2s;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public class UpdatePacketSender : MonoBehaviour
	{
		private Stopwatch stopwatch = Stopwatch.StartNew();

		public UpdatePacketSender(IntPtr ptr) : base(ptr) {}


		public static void Setup()
		{
			var updatePacketSenderGameObject = new GameObject("UpdatePacketSender");
			updatePacketSenderGameObject.AddComponent<UpdatePacketSender>();

			updatePacketSenderGameObject.transform.parent = PMFHelper.SystemTransform;

			
		}


		private void FixedUpdate()
		{
			//TODO: Get from server primsconfig.json
			if(stopwatch.ElapsedMilliseconds >= 10)
			{
				stopwatch.Restart();
				
				if(MultiplayerManager.Client == null || !MultiplayerManager.Client.IsInGame)
				{
					return;
				}

				var packet = new PlayerUpdatePacket()
				{
					Position = PMFHelper.CameraRig.transform.position.ToNumerics(),
					HeadPosition = Camera.main.transform.position.ToNumerics(),
					LHandPosition = PMFHelper.LHand.transform.position.ToNumerics(),
					RHandPosition = PMFHelper.RHand.transform.position.ToNumerics()

				};


				MultiplayerManager.Client.SendPacket(packet, LiteNetLib.DeliveryMethod.Unreliable);
			}

		}
	}
}
