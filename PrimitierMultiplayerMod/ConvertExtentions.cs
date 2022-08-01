using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class ConvertExtentions
	{
		public static System.Numerics.Vector3 ToNumerics(this UnityEngine.Vector3 vector3)
		{
			return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
		}

		public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 vector3)
		{
			return new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
		}

		public static System.Numerics.Quaternion ToNumerics(this UnityEngine.Quaternion quaternion)
		{
			return new System.Numerics.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
		}

		public static UnityEngine.Quaternion ToUnity(this System.Numerics.Quaternion quaternion)
		{
			return new UnityEngine.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
		}
	}
}
