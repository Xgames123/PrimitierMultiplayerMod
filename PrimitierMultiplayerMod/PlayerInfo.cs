using MelonLoader;
using System;

namespace PrimitierMultiplayerMod
{
	public static class PlayerInfo
	{
		public static string Username { get { return UsernameEntry.Value; } }
		public static MelonPreferences_Entry<string> UsernameEntry;

		public static string StaticId;
		public static MelonPreferences_Entry<string> StaticIdEntry;

		private static MelonPreferences_Category _playerInfoCategory = null;

		public static void Load()
		{
			if (_playerInfoCategory == null)
			{
				_playerInfoCategory = MelonPreferences.CreateCategory("PlayerInfo");
				UsernameEntry = _playerInfoCategory.CreateEntry("Username", "username123");

				StaticIdEntry = _playerInfoCategory.CreateEntry("StaticPlayerId", "");
				if (string.IsNullOrEmpty(StaticIdEntry.Value))
				{
					StaticIdEntry.Value = Guid.NewGuid().ToString();
				}
				StaticId = StaticIdEntry.Value;
			}


		}

	}
}
