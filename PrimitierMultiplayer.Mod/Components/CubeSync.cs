using LiteNetLib;
using PrimitierModdingFramework;
using PrimitierModdingFramework.SubstanceModding;
using PrimitierMultiplayer.Shared;
using PrimitierMultiplayer.Shared.Models;
using PrimitierMultiplayer.Shared.Packets.c2s2c;
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
			sync.AddToChunk(sync._currentChunk);
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
			RemoveFromChunk(_currentChunk);
			CubeSyncList.Remove(Id);
		}

		public void DestroyCube()
		{
			Destroy(gameObject);
			//Further handling is done in OnDestroy
		}



		private RuntimeChunk AddToChunk(System.Numerics.Vector2 chunkPos)
		{
			var chunk = WorldManager.GetChunk(chunkPos);
			chunk.NetworkSyncs.Add(Id);
			return chunk;
		}
		private void RemoveFromChunk(System.Numerics.Vector2 chunkPos)
		{
			var chunk = WorldManager.GetChunk(chunkPos);
			if(chunk == null)
			{
				return;
			}
			chunk.NetworkSyncs.Remove(Id);

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
				var newChunk = AddToChunk(chunkPos);
				RemoveFromChunk(_currentChunk);
				
				
				if(newChunk.Owner != MultiplayerManager.LocalId)
				{
					DestroyCube();
					MultiplayerManager.Client.SendPacket(new CubeChunkChangePacket()
					{
						OldChunk = chunkPos,
						NewChunk = _currentChunk,
						Cube = ToNetworkCube()
					}, DeliveryMethod.ReliableOrdered);
				}
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
