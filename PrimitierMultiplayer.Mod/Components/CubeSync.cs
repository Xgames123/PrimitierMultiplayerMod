using LiteNetLib;
using PrimitierModdingFramework;
using PrimitierModdingFramework.SubstanceModding;
using PrimitierMultiplayer;
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
			sync.TryRecalculateCurrentChunk(chunkPos);
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
			var currentChunk = WorldManager.GetChunk(_currentChunk);
			if(currentChunk != null)
				currentChunk.NetworkSyncs.Remove(Id);

			CubeSyncList.Remove(Id);
		}

		public void DestroyCube()
		{
			Destroy(gameObject);
			//Further handling is done in OnDestroy
		}



		/// <summary>
		/// Returns true when it needs to move the object between chunks of different owners
		/// </summary>
		/// <param name="chunkPos"></param>
		/// <param name="newChunk"></param>
		/// <param name="oldChunk"></param>
		/// <returns></returns>
		private bool TryRecalculateCurrentChunk(System.Numerics.Vector2 chunkPos, bool destroyCubeOnFail=true)
		{

			if (chunkPos != _currentChunk)
			{
				var newChunk = WorldManager.GetChunk(chunkPos);
				var oldChunk = WorldManager.GetChunk(_currentChunk);
				_currentChunk = chunkPos;

				if (newChunk.Owner != MultiplayerManager.LocalId)
				{
					if (destroyCubeOnFail)
						DestroyCube();
					return false;
				}
				

				oldChunk.NetworkSyncs.Remove(Id);
				newChunk.NetworkSyncs.Add(Id);

			}

			return true;
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
				Substance = (int)CubeBase.substance

			};

			var chunkPos = ChunkMath.WorldToChunkPos(transform.position.ToNumerics());
			if (TryRecalculateCurrentChunk(chunkPos))
			{
				PMFLog.Message($"Moving object Id:{Id} to chunk X: {chunkPos.X}, Y: {chunkPos.Y} from X: {_currentChunk.X}, Y: {_currentChunk.Y}");
				MultiplayerManager.Client.SendPacket(new CubeChunkChangePacket()
				{
					OldChunk = _currentChunk,
					NewChunk = chunkPos,
					Cube = netCube,
				}, DeliveryMethod.ReliableOrdered);
				DestroyCube();
			}


			return netCube;



		}


		public void UpdateFromServer(NetworkCube cube, System.Numerics.Vector2 chunkPos)
		{
			if (!IsValid())
				return;

			TryRecalculateCurrentChunk(chunkPos);

			CubeBase.transform.position = cube.Position.ToUnity();
			CubeBase.transform.rotation = cube.Rotation.ToUnity();
			CubeBase.ChangeScale(cube.Size.ToUnity());
			CubeBase.rb.velocity = cube.Velosity.ToUnity();
			CubeBase.rb.angularVelocity = cube.AngularVelocity.ToUnity();
			CubeBase.ChangeSubstance((Substance)cube.Substance);
			
			

		}

		

	}
}
