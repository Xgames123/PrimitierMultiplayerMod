using PrimitierModdingFramework;
using PrimitierMultiplayer.Mod.Interpolation;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayer.Mod.Components
{
	public class RemotePlayer : MonoBehaviour
	{
		private const float InterpolationSmoothing = 5;

		public RemotePlayer(IntPtr ptr) : base(ptr) { }

		public static Dictionary<int, RemotePlayer> RemotePlayers = new Dictionary<int, RemotePlayer>();

		public Vector3Interpolator<SmoothInterpolator> PositionInterpolator = new Vector3Interpolator<SmoothInterpolator>();
		public Vector3Interpolator<SmoothInterpolator> HeadInterpolator = new Vector3Interpolator<SmoothInterpolator>();
		public Vector3Interpolator<SmoothInterpolator> LHandInterpolator = new Vector3Interpolator<SmoothInterpolator>();
		public Vector3Interpolator<SmoothInterpolator> RHandInterpolator = new Vector3Interpolator<SmoothInterpolator>();

		public Transform Head;
		public Transform LHand;
		public Transform RHand;
		public TextMeshPro FirstPersonNameTag;
		public TextMeshPro ThirdPersonNameTag;

		public int Id;
		

		public void TeleportInterpolators()
		{
			PositionInterpolator.Teleport(transform.position);
			HeadInterpolator.Teleport(Head.position);
			LHandInterpolator.Teleport(LHand.position);
			RHandInterpolator.Teleport(RHand.position);
		}
		

		public void Sync(NetworkPlayer networkPlayer)
		{
			PositionInterpolator.SetTarget(networkPlayer.Position.ToUnity());
			HeadInterpolator.SetTarget(networkPlayer.HeadPosition.ToUnity());
			LHandInterpolator.SetTarget(networkPlayer.LHandPosition.ToUnity());
			RHandInterpolator.SetTarget(networkPlayer.RHandPosition.ToUnity());

		}

		private void Update()
		{
			transform.position = PositionInterpolator.GetCurrentValue(Time.deltaTime);
			Head.position = HeadInterpolator.GetCurrentValue(Time.deltaTime);
			LHand.position = LHandInterpolator.GetCurrentValue(Time.deltaTime);
			RHand.position = RHandInterpolator.GetCurrentValue(Time.deltaTime);
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

		public static void Create(InitialPlayerData initialPlayerData, bool addJoinedMessageToChat=true)
		{
			if(addJoinedMessageToChat)
				Mod.Chat.AddServerMessage($"{initialPlayerData.Username} has Joined the game");

			Create(initialPlayerData.Id, initialPlayerData.Username, initialPlayerData.Position.ToUnity());
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

			var firstPersonNameTag = CreateNameTag(username, headGo.transform, 9, Camera.main, "FirstPerson_NameTag");


			var remotePlayer = remotePlayerGo.AddComponent<RemotePlayer>();
			remotePlayer.Head = headGo.transform;
			remotePlayer.LHand = LHandGo.transform;
			remotePlayer.RHand = RHandGo.transform;
			remotePlayer.FirstPersonNameTag = firstPersonNameTag;
			remotePlayer.Id = id;
			

			remotePlayerGo.transform.position = position;

			RemotePlayers.Add(id, remotePlayer);
			return remotePlayer;
		}


		private static TextMeshPro CreateNameTag(string username, Transform parent, int layer, Camera camera, string goName)
		{
			var nameTagGo = new GameObject(goName);
			nameTagGo.layer = layer;
			nameTagGo.transform.parent = parent;
			nameTagGo.transform.localPosition = new Vector3(0, 0.6f, 0);
			nameTagGo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			var nametagComp = nameTagGo.AddComponent<NameTag>();
			nametagComp.TargetCamera = camera;
			var nameTag = nameTagGo.AddComponent<TextMeshPro>();
			nameTag.alignment = TextAlignmentOptions.Center;
			nameTag.color = Color.white;
			nameTagGo.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
			nameTag.text = username;
			nameTag.outlineColor = Color.black;
			nameTag.outlineWidth = 1f;

			return nameTag;
		}

		private static Camera FindThirdPersonCamera()
		{
			var camera = GameObject.Find("ThirdPersonUICamera");
			if (camera == null)
				return null;

			return camera.GetComponent<Camera>();


		}
	}
}
