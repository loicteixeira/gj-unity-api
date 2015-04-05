using UnityEngine;
using System.Collections;

namespace GJAPI
{
	public static class Constants
	{
		public const string SETTINGS_ASSET_NAME = "GJAPISettings";
		public const string SETTINGS_ASSET_FULL_NAME = SETTINGS_ASSET_NAME + ".asset";
		public const string SETTINGS_ASSET_PATH = "Assets/Plugins/GameJolt/Resources/";
		public const string SETTINGS_ASSET_FULL_PATH = SETTINGS_ASSET_PATH + SETTINGS_ASSET_FULL_NAME;

		public const string MANAGER_NAME = "Game Jolt API Manager";

		public const string API_PROTOCOL = "http://";
		public const string API_ROOT = "gamejolt.com/api/game/";
		public const string API_VERSION = "1";
		public const string API_BASE_URL = API_PROTOCOL + API_ROOT + "v" + API_VERSION + "/";

		public const string API_USERS_AUTH = "users/auth/";
		public const string API_USERS_FETCH = "users/";

		public const string API_SESSIONS_OPEN = "sessions/open/";
		public const string API_SESSIONS_PING = "sessions/ping/";
		public const string API_SESSIONS_CLOSE = "sessions/close/";

		public const string API_SCORES_ADD = "scores/add/";
		public const string API_SCORES_FETCH = "scores/";
		public const string API_SCORES_TABLES_FETCH = "scores/tables/";

		public const string API_TROPHIES_ADD = "trophies/add-achieved/";
		public const string API_TROPHIES_FETCH = "trophies/";

		public const string API_DATASTORE_SET = "data-store/set/";
		public const string API_DATASTORE_UPDATE = "data-store/update/";
		public const string API_DATASTORE_FETCH = "data-store/";
		public const string API_DATASTORE_REMOVE = "data-store/remove/";
		public const string API_DATASTORE_KEYS_FETCH = "data-store/get-keys/";
	}
}
