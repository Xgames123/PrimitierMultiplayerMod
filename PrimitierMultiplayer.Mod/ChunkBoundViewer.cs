using PrimitierMultiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PrimitierModdingFramework;
using PrimitierMultiplayer.Shared;

namespace PrimitierMultiplayer.Mod
{
	public class ChunkBoundViewer : MonoBehaviour
	{
		public static bool IsCreated { get; private set; } = false;

		public ChunkBoundViewer(IntPtr ptr) : base(ptr) { }

		public void FixedUpdate()
		{
			
			var playerPos = Camera.main.transform.position;
			transform.position = ChunkMath.ChunkToWorldPos(ChunkMath.WorldToChunkPos(playerPos.ToNumerics())).ToUnity();

		}

		public static void Create()
		{
			if (IsCreated)
				return;

			var chunkBoundViewer = new GameObject("ChunkBoundViewer");
			chunkBoundViewer.AddComponent<ChunkBoundViewer>();

			var chunkSize = CubeGenerator.chunkTileCount * TerrainMeshGenerator.tileLength;

			var cube00 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube00.transform.parent = chunkBoundViewer.transform;
			cube00.transform.localPosition = Vector3.zero;
			cube00.transform.localScale = new Vector3(0.05f, 500, 0.05f);
			cube00.GetComponent<MeshRenderer>().material.color = Color.red;
			cube00.GetComponent<MeshRenderer>().castShadows = false;
			Destroy(cube00.GetComponent<BoxCollider>());

			var cube10 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube10.transform.parent = chunkBoundViewer.transform;
			cube10.transform.localPosition = new Vector3(chunkSize, 0, 0);
			cube10.transform.localScale = new Vector3(0.05f, 500, 0.05f);
			cube10.GetComponent<MeshRenderer>().material.color = Color.red;
			cube10.GetComponent<MeshRenderer>().castShadows = false;
			Destroy(cube10.GetComponent<BoxCollider>());

			var cube01 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube01.transform.parent = chunkBoundViewer.transform;
			cube01.transform.localPosition = new Vector3(0, 0, chunkSize);
			cube01.transform.localScale = new Vector3(0.05f, 500, 0.05f);
			cube01.GetComponent<MeshRenderer>().material.color = Color.red;
			cube01.GetComponent<MeshRenderer>().castShadows = false;
			Destroy(cube01.GetComponent<BoxCollider>());

			var cube11 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube11.transform.parent = chunkBoundViewer.transform;
			cube11.transform.localPosition = new Vector3(chunkSize, 0, chunkSize);
			cube11.transform.localScale = new Vector3(0.05f, 500, 0.05f);
			cube11.GetComponent<MeshRenderer>().material.color = Color.red;
			cube11.GetComponent<MeshRenderer>().castShadows = false;
			Destroy(cube11.GetComponent<BoxCollider>());


			IsCreated = true;
		}
		public static void Remove()
		{
			if (!IsCreated)
				return;
			
			Destroy(GameObject.Find("ChunkBoundViewer"));
			IsCreated = false;
		}

	}
}
