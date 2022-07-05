using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PrimitierMultiplayerMod
{
	public class RemotePlayer : MonoBehaviour
	{
		public RemotePlayer(System.IntPtr ptr) : base(ptr) { }

		public Transform Head;
		public Transform LHand;
		public Transform RHand;
		public TextMeshPro NameTag;

		public int Id;


		public static RemotePlayer Create(int id, string username, Vector3 position)
		{
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
			nameTag.text = "Null";
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

			return remotePlayer;
		}

	}
}
