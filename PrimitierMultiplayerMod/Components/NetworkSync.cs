using PrimitierModdingFramework;
using PrimitierModdingFramework.SubstanceModding;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayer.Mod.Components
{
	public class NetworkSync : MonoBehaviour
	{
		public NetworkSync(System.IntPtr ptr) :base(ptr) { }

		public static Dictionary<uint, NetworkSync> NetworkSyncList = new Dictionary<uint, NetworkSync>();

		public static NetworkSync GetById(uint id)
		{
			if(NetworkSyncList.TryGetValue(id, out NetworkSync networkSync))
			{
				return networkSync;
			}
			return null;
		}


		public static void Register(NetworkSync sync, System.Numerics.Vector2 chunkPos)
		{
			sync._currentChunk = chunkPos;
			NetworkSyncList.Add(sync.Id, sync);
			AddToChunk(sync._currentChunk, sync.Id);
		}


		public uint Id;


		public CubeBase CubeBase;
		private System.Numerics.Vector2 _currentChunk;
		public void Start()
		{

			CubeBase = GetComponent<CubeBase>();

		}
		public void OnDestroy()
		{
			RemoveFromChunk(_currentChunk, Id);
			NetworkSyncList.Remove(Id);
		}

		public void DestroyCube()
		{
			Destroy(gameObject);
			//Further handling is done in OnDestroy
		}



		private static void AddToChunk(System.Numerics.Vector2 chunkPos, uint id)
		{
			var chunk = WorldManager.GetChunk(chunkPos);
			chunk.NetworkSyncs.Add(id);
		}
		private static void RemoveFromChunk(System.Numerics.Vector2 chunkPos, uint id)
		{
			var chunk = WorldManager.GetChunk(chunkPos);
			if(chunk == null)
			{
				return;
			}
			chunk.NetworkSyncs.Remove(id);

		}

		public void UpdateSync(NetworkCube cube, System.Numerics.Vector2 chunkPos)
		{
			if (!NetworkSyncList.ContainsKey(Id) || CubeBase == null)
				return;

			if (chunkPos != _currentChunk)
			{
				AddToChunk(chunkPos, Id);
				RemoveFromChunk(_currentChunk, Id);
				_currentChunk = chunkPos;
			}


			CubeBase.ChangeScale(cube.Size.ToUnity());
			CubeBase.ChangeSubstance((Substance)cube.Substance);
			CubeBase.transform.position = cube.Position.ToUnity();
			CubeBase.transform.rotation = cube.Rotation.ToUnity();
			CubeBase.rb.velocity = cube.Velosity.ToUnity();
			CubeBase.rb.angularVelocity = cube.AngularVelocity.ToUnity();
		}

		

	}
}
