using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PrimitierMultiplayer.Server
{
	public static class ChunkMath
	{
		public const int TileLength = 4;
		public const int ChunkTileCount = 4;
		public static Vector2 WorldOriginOffset = Vector2.Zero;

		public static Vector2 WorldToChunkPos(Vector3 pos)
		{

			//This is always 0 because WorldOriginOffset is zero
			//var chunkPosOffset = WorldOriginOffset * TerrainGenerator.areaTileCount / ChunkTileCount;
			var chunkPosOffset = Vector2.Zero;

			var chunkPosX = MathF.Floor(pos.X / ChunkTileCount / TileLength);
			var chunkPosY = MathF.Floor(pos.Z / ChunkTileCount / TileLength);
			return new Vector2(chunkPosX, chunkPosY) + chunkPosOffset;
		}

		public static Vector3 ChunkToWorldPos(Vector2 chunkPos)
		{
			//This is always 0 because WorldOriginOffset is zero
			//var chunkPosOffset = WorldOriginOffset * TerrainGenerator.areaTileCount / ChunkTileCount;
			var chunkPosOffset = Vector2.Zero;

			return new Vector3(chunkPos.X - chunkPosOffset.X, 0, chunkPos.Y - chunkPosOffset.Y) * ChunkTileCount * TileLength;
		}


		public static float WorldToChunkRadius(float radius)
		{
			var chunkPosOffset = 0;

			var chunkPosX = MathF.Floor(radius / ChunkTileCount / TileLength);
			return chunkPosX + chunkPosOffset;
		}
	}
}
