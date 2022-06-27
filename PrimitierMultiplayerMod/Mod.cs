using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

namespace PrimitierMultiplayerMod
{

	public class Mod : PrimitierMod
    {



		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			PMFLog.Message("Hello Primitier!");
			PMFLog.Message("Mod version " + Assembly.GetName().Version);
			
		}
		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			PMFSystem.EnableSystem<PMFHelper>();
		}
		
		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
		}


	}
}
