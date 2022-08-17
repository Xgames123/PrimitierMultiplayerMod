using PrimitierModdingFramework;
using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayerMod.Components
{
	public class RemotePlayer : MonoBehaviour
	{
		private const float InterpolationSmoothing = 5;

		public RemotePlayer(IntPtr ptr) : base(ptr) { }

		public static Dictionary<int, RemotePlayer> RemotePlayers = new Dictionary<int, RemotePlayer>();

		public Vector3 TargetPosition;
		public Vector3 TargetHeadPosition;
		public Vector3 TargetLHandPosition;
		public Vector3 TargetRHandPosition;

		public Transform Head;
		public Transform LHand;
		public Transform RHand;
		public TextMeshPro NameTag;

		public int Id;

		

		public void Sync(NetworkPlayer networkPlayer)
		{
			TargetPosition = networkPlayer.Position.ToUnity();
			TargetHeadPosition = networkPlayer.HeadPosition.ToUnity();
			TargetLHandPosition = networkPlayer.LHandPosition.ToUnity();
			TargetRHandPosition = networkPlayer.RHandPosition.ToUnity();

		}

		private void Update()
		{
			transform.position = Vector3.Lerp(transform.position, TargetPosition, Mathf.SmoothStep(0, 1, Time.deltaTime* InterpolationSmoothing));
			Head.position = Vector3.Lerp(Head.position, TargetHeadPosition, Mathf.SmoothStep(0, 1, Time.deltaTime* InterpolationSmoothing));
			LHand.position = Vector3.Lerp(LHand.position, TargetLHandPosition, Mathf.SmoothStep(0, 1, Time.deltaTime* InterpolationSmoothing));
			RHand.position = Vector3.Lerp(RHand.position, TargetRHandPosition, Mathf.SmoothStep(0, 1, Time.deltaTime* InterpolationSmoothing));
		}


		public static void DeletePlayer(RemotePlayer player)
		{
			if (player != null)
			{
				Destroy(player.gameObject);
				RemotePlayers.Remove(player.Id);
			}

		}

		public static void DeleteAllPlayers()
		{
			foreach (var player in RemotePlayers.Values.ToArray())
			{
				DeletePlayer(player);
			}
		}



		public static RemotePlayer Create(int id, string username, Vector3 position)
		{
			if (RemotePlayers.ContainsKey(id))
				return null;

			var remotePlayerGo = new GameObject("RemotePlayer");

			var headGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			headGo.name = "Head";
			headGo.transform.parent = remotePlayerGo.transform;
			headGo.transform.localPosition = Vector3.zero;
			headGo.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
			Destroy(headGo.GetComponent<SphereCollider>());

			var nameTagGo = new GameObject("NameTag");
			nameTagGo.transform.parent = headGo.transform;
			nameTagGo.transform.localPosition = new Vector3(0, 0.6f, 0);
			nameTagGo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			nameTagGo.AddComponent<NameTag>();
			var nameTag = nameTagGo.AddComponent<TextMeshPro>();
			nameTag.alignment = TextAlignmentOptions.Center;
			nameTag.color = Color.white;
			nameTagGo.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
			nameTag.text = username;
			nameTag.outlineColor = Color.black;
			nameTag.outlineWidth = 1f;


			var LHandGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			headGo.name = "LHand";
			LHandGo.transform.parent = remotePlayerGo.transform;
			LHandGo.transform.localPosition = Vector3.zero;
			LHandGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			Destroy(LHandGo.GetComponent<SphereCollider>());

			var RHandGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			headGo.name = "RHand";
			RHandGo.transform.parent = remotePlayerGo.transform;
			RHandGo.transform.localPosition = Vector3.zero;
			RHandGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			Destroy(RHandGo.GetComponent<SphereCollider>());

			var remotePlayer = remotePlayerGo.AddComponent<RemotePlayer>();
			remotePlayer.Head = headGo.transform;
			remotePlayer.LHand = LHandGo.transform;
			remotePlayer.RHand = RHandGo.transform;
			remotePlayer.NameTag = nameTag;
			remotePlayer.Id = id;

			remotePlayerGo.transform.position = position;

			RemotePlayers.Add(id, remotePlayer);
			return remotePlayer;
		}

	}
}
