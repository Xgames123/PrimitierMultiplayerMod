using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer
{
	public class RuntimePlayer
	{
		public RuntimePlayer(string username, int id)
		{
			Username = username;
			RuntimeId = id;
		}

		public string Username;
		public int RuntimeId;
		public string StaticId;

		public float Hp;

		public Vector3 Position = Vector3.Zero;
		public Vector3 HeadPosition = Vector3.Zero;
		public Vector3 LHandPosition = Vector3.Zero;
		public Vector3 RHandPosition = Vector3.Zero;
	}

}
