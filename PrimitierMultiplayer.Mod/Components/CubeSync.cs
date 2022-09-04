using LiteNetLib;
using PrimitierModdingFramework;
using PrimitierModdingFramework.SubstanceModding;
using PrimitierMultiplayer;
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


		public static void Register(CubeSync sync)
		{
			CubeSyncList.Add(sync.Id, sync);
			
		}

		private void FixedUpdate()
		{
			RecalculateChunkPosition();
			
		}

		public uint Id;


		public CubeBase CubeBase;
		private System.Numerics.Vector2 _currentChunk;
		public bool IsInWrongChunk { get; private set; } = false;
		public void Start()
		{

			CubeBase = GetComponent<CubeBase>();

		}
		public void OnDestroy()
		{
			var currentChunk = WorldManager.GetVisibleChunk(_currentChunk);
			if(currentChunk != null)
				currentChunk.NetworkSyncs.Remove(Id);

			CubeSyncList.Remove(Id);
		}

		public void DestroyCube()
		{
			Destroy(gameObject);
			//Further handling is done in OnDestroy
		}


		private void RecalculateChunkPosition()
		{
			
			var chunkPos = ChunkMath.WorldToChunkPos(transform.position.ToNumerics());
			var chunk = WorldManager.GetVisibleChunk(chunkPos);
			if (chunk == null)
				DestroyCube();

			if (chunk.Owner != MultiplayerManager.LocalId)
			{
				IsInWrongChunk = true;
				return;
			}
			IsInWrongChunk = false;
			if (!chunk.NetworkSyncs.Contains(Id))
			{
				var oldChunk = WorldManager.GetVisibleChunk(_currentChunk);
				if (oldChunk != null)
					oldChunk.NetworkSyncs.Remove(Id);

				chunk.NetworkSyncs.Add(Id);
				_currentChunk = chunkPos;
			}



		}


		public bool IsValid()
		{
			if (CubeSyncList.ContainsKey(Id) && CubeBase != null)
				return true;

			return false;
		}

		public NetworkCube UpdateToServer()
		{
			if (!IsValid())
				return default;

			

			var netCube = new NetworkCube()
			{
				Id = Id,
				Position = CubeBase.transform.position.ToNumerics(),
				Rotation = CubeBase.transform.rotation.ToNumerics(),
				Size = CubeBase.transform.localScale.ToNumerics(),
				Velosity = CubeBase.rb.velocity.ToNumerics(),
				AngularVelocity = CubeBase.rb.angularVelocity.ToNumerics(),
				Substance = (int)CubeBase.substance,
				IsInWrongChunk = IsInWrongChunk
			};

			return netCube;


			if (IsInWrongChunk)
			{
				DestroyCube();
			}
		}


		public void UpdateFromServer(NetworkCube cube)
		{
			if (!IsValid())
				return;

			CubeBase.rb.position = cube.Position.ToUnity();
			CubeBase.rb.rotation = cube.Rotation.ToUnity();
			CubeBase.ChangeScale(cube.Size.ToUnity());
			CubeBase.rb.velocity = cube.Velosity.ToUnity();
			CubeBase.rb.angularVelocity = cube.AngularVelocity.ToUnity();
			CubeBase.ChangeSubstance((Substance)cube.Substance);

		}

		

	}
}
