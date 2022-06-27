using System;
using System.Collections.Generic;
using System.Linq;
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

	public Vector3 Position = Vector3.Zero;
}
