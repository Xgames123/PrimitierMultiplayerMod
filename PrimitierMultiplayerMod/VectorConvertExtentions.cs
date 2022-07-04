using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class VectorConvertExtentions
	{
		public static System.Numerics.Vector3 ToNumerics(this UnityEngine.Vector3 vector3)
		{
			return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
		}

		public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 vector3)
		{
			return new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
		}
	}
}
