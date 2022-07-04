using PrimitierModdingFramework;
using PrimS.shared.Packets.c2s;
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
			if(stopwatch.ElapsedMilliseconds >= 10)
			{
				stopwatch.Restart();
				
				if(Mod.Client == null || !Mod.Client.IsInGame)
				{
					return;
				}

				var playerPos = PMFHelper.CameraRig.transform.position.ToNumerics();
				Mod.Client.SendPacket(new PlayerUpdatePacket()
				{
					Position = playerPos,
					HeadPosition = Camera.main.transform.position.ToNumerics(),
					LHandPosition = playerPos-PMFHelper.LHand.transform.position.ToNumerics(),
					RHandPosition = playerPos - PMFHelper.RHand.transform.position.ToNumerics()

				}, LiteNetLib.DeliveryMethod.Unreliable);
			}

		}
	}
}
