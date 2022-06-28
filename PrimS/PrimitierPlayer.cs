using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrimS;
public class PrimitierPlayer
{
	public PrimitierPlayer(string username, int id)
	{
		Username = username;
		Id = id;
	}

	public string Username;
	public int Id;

	public Vector3 HeadPosition = Vector3.Zero;
	public Vector3 LHandPosition = Vector3.Zero;
	public Vector3 RHandPosition = Vector3.Zero;
}
