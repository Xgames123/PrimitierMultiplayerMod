using MelonLoader;

namespace PrimitierMultiplayerMod
{
	public static class PlayerInfo
	{
		public static string Username { get { return UsernameEntry.Value; } }
		public static MelonPreferences_Entry<string> UsernameEntry;


		private static MelonPreferences_Category _playerInfoCategory = null;

		public static void Load()
		{
			if (_playerInfoCategory == null)
			{
				_playerInfoCategory = MelonPreferences.CreateCategory("PlayerInfo");
				UsernameEntry = _playerInfoCategory.CreateEntry("Username", "username123");
			}


		}

	}
}
