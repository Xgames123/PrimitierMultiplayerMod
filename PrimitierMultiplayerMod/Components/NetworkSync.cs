using PrimitierModdingFramework;
using PrimitierModdingFramework.SubstanceModding;
using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod.Components
{
	public class NetworkSync : MonoBehaviour
	{
		public NetworkSync(System.IntPtr ptr) :base(ptr) { }

		public static Dictionary<System.Numerics.Vector2, List<uint>> NetworkSyncChunk = new Dictionary<System.Numerics.Vector2, List<uint>>();
		public static Dictionary<uint, NetworkSync> NetworkSyncList = new Dictionary<uint, NetworkSync>();

		public static List<NetworkSync> GetSyncsInChunk(System.Numerics.Vector2 chunkPos)
		{
			var outList = new List<NetworkSync>();
			foreach (var syncId in GetChunkIdList(chunkPos))
			{
				if(NetworkSyncList.TryGetValue(syncId, out NetworkSync sync))
				{
					outList.Add(sync);
				}
			}
			return outList;
		}

		public static void Register(NetworkSync sync)
		{
			sync._currentChunk = ((Vector2)CubeGenerator.WorldToChunkPos(sync.transform.position)).ToNumerics();
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
			PMFLog.Message("Cube destroyed");
		}

		public void DestroyCube()
		{
			Destroy(gameObject);
		}


		private static List<uint> GetChunkIdList(System.Numerics.Vector2 chunk)
		{
			List<uint> list;
			if (NetworkSyncChunk.TryGetValue(chunk, out List<uint> getList))
			{
				list = getList;
			}
			else
			{
				var newList = new List<uint>();
				list = newList;
				NetworkSyncChunk.Add(chunk, newList);
			}
			return list;
		}

		private static void AddToChunk(System.Numerics.Vector2 chunk, uint id)
		{
			var list = GetChunkIdList(chunk);
			list.Add(id);
		}
		private static void RemoveFromChunk(System.Numerics.Vector2 chunk, uint id)
		{
			var list = GetChunkIdList(chunk);
			list.Remove(id);
		}

		public void UpdateSync(NetworkCube cube)
		{
			if (!NetworkSyncList.ContainsKey(Id) || CubeBase == null)
				return;

			var newChunk = ((UnityEngine.Vector2)CubeGenerator.WorldToChunkPos(transform.position)).ToNumerics();
			if (newChunk != _currentChunk)
			{
				AddToChunk(newChunk, Id);
				RemoveFromChunk(_currentChunk, Id);
				_currentChunk = newChunk;
			}

			CubeBase.ChangeScale(cube.Size.ToUnity());
			CubeBase.ChangeSubstance((Substance)cube.Substance);
			CubeBase.transform.position = cube.Position.ToUnity();
			CubeBase.transform.rotation = cube.Rotation.ToUnity();
		}

		

	}
}
