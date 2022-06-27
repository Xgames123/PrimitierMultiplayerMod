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
	public PrimitierPlayer(string username, int id, IPEndPoint endPoint)
	{
		Username = username;
		Id = id;
		Endpoint = endPoint;
	}

	public IPEndPoint Endpoint;
	public string Username;
	public int Id;

	public Vector3 Position = Vector3.Zero;
}
