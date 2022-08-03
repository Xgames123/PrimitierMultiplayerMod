using PrimitierModdingFramework.SubstanceModding;
using PrimitierServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimitierMultiplayerMod.Components
{
	public class NetworkSync : MonoBehaviour, ICustomCubeBehaviour
	{
		public NetworkSync(System.IntPtr ptr) :base(ptr) { }

		public static Dictionary<uint, NetworkSync> NetworkSyncs = new Dictionary<uint, NetworkSync>();

		public uint Id;


		private CubeBase cubeBase;
		public void Start()
		{
			cubeBase = GetComponent<CubeBase>();
			NetworkSyncs.Add(Id, this);
		}

		public void UpdateSync(NetworkCube cube)
		{
			cubeBase.ChangeScale(cube.Size.ToUnity());
			cubeBase.ChangeSubstance((Substance)cube.Substance);
			cubeBase.transform.position = cube.Position.ToUnity();
			cubeBase.transform.rotation = cube.Rotation.ToUnity();
		}

		public void OnDestroy()
		{
			NetworkSyncs.Remove(Id);
		}

		

	}
}
