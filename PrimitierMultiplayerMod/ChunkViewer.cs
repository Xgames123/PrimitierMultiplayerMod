using PrimitierMultiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PrimitierModdingFramework;

namespace PrimitierMultiplayer.Mod
{
	public class ChunkViewer : MonoBehaviour
	{

		public ChunkViewer(IntPtr ptr) : base(ptr) { }

		public void FixedUpdate()
		{
			var playerPos = Camera.main.transform.position;
			transform.position = (Vector2)CubeGenerator.WorldToChunkPos(playerPos);

		}

		public static void Create()
		{
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "ChunkViewer";
			cube.transform.localScale = new Vector3(0.05f, 10, 0.05f);
			cube.GetComponent<MeshRenderer>().material.color = Color.red;
			Destroy(cube.GetComponent<BoxCollider>());
			cube.AddComponent<ChunkViewer>();
		}

	}
}
