using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayer.Server
{
	public class SupportedVersions
	{
		public static Version LowestSupportedModVersion = new Version(0, 0, 1);





		public static bool CheckModVersion(Version modVersion)
		{
			if (modVersion < LowestSupportedModVersion)
				return false;

			return true;
		}
	}
}

