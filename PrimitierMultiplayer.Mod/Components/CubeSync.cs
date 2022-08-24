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
	public class CubeSync : MonoBehaviour
	{
		public CubeSync(System.IntPtr ptr) :base(ptr) { }

		public static Dictionary<uint, CubeSync> CubeSyncList = new Dictionary<uint, CubeSync>();

		public static CubeSync GetById(uint id)
		{
			if(CubeSyncList.TryGetValue(id, out CubeSync cubeSync))
			{
				return cubeSync;
			}
			return null;
		}


		public static void Register(CubeSync sync, System.Numerics.Vector2 chunkPos)
		{
			sync._currentChunk = chunkPos;
			CubeSyncList.Add(sync.Id, sync);
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
			CubeSyncList.Remove(Id);
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

		public bool IsValid()
		{
			if (CubeSyncList.ContainsKey(Id) && CubeBase != null)
				return true;

			return false;
		}

		public NetworkCube ToNetworkCube()
		{
			if (!IsValid())
				return default;

			return new NetworkCube()
			{
				Id = Id,
				Position= CubeBase.transform.position.ToNumerics(),
				Rotation= CubeBase.transform.rotation.ToNumerics(),
				Size = CubeBase.transform.localScale.ToNumerics(),
				Velosity = CubeBase.rb.velocity.ToNumerics(),
				AngularVelocity = CubeBase.rb.angularVelocity.ToNumerics(),
				Substance = (int)CubeBase.substance

			};

		}


		public void UpdateSync(NetworkCube cube, System.Numerics.Vector2 chunkPos)
		{
			if (!IsValid())
				return;

			if (chunkPos != _currentChunk)
			{
				AddToChunk(chunkPos, Id);
				RemoveFromChunk(_currentChunk, Id);
				_currentChunk = chunkPos;
			}

			CubeBase.transform.position = cube.Position.ToUnity();
			CubeBase.transform.rotation = cube.Rotation.ToUnity();
			CubeBase.ChangeScale(cube.Size.ToUnity());
			CubeBase.rb.velocity = cube.Velosity.ToUnity();
			CubeBase.rb.angularVelocity = cube.AngularVelocity.ToUnity();
			CubeBase.ChangeSubstance((Substance)cube.Substance);
			
		}

		

	}
}
