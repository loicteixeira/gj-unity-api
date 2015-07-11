using UnityEngine;
using System.Collections;

namespace GameJolt.API
{
	public static class Constants
	{
		public const string VERSION = "2.0.1";

		public const string SETTINGS_ASSET_NAME = "GJAPISettings";
		public const string SETTINGS_ASSET_FULL_NAME = SETTINGS_ASSET_NAME + ".asset";
		public const string SETTINGS_ASSET_PATH = "Assets/Plugins/GameJolt/Resources/";
		public const string SETTINGS_ASSET_FULL_PATH = SETTINGS_ASSET_PATH + SETTINGS_ASSET_FULL_NAME;

		public const string MANAGER_ASSET_NAME = "GameJoltAPI";
		public const string MANAGER_ASSET_FULL_NAME = MANAGER_ASSET_NAME + ".prefab";
		public const string MANAGER_ASSET_PATH = "Assets/Plugins/GameJolt/Prefabs/";
		public const string MANAGER_ASSET_FULL_PATH = MANAGER_ASSET_PATH + MANAGER_ASSET_FULL_NAME;

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

		public const string IMAGE_RESOURCE_REL_PATH = "Images/";

		public const string DEFAULT_AVATAR_ASSET_NAME = "GJAPIDefaultAvatar";
		public const string DEFAULT_AVATAR_ASSET_PATH = IMAGE_RESOURCE_REL_PATH + DEFAULT_AVATAR_ASSET_NAME;
		public const string DEFAULT_NOTIFICATION_ASSET_NAME = "GJAPIDefaultNotification";
		public const string DEFAULT_NOTIFICATION_ASSET_PATH = IMAGE_RESOURCE_REL_PATH + DEFAULT_NOTIFICATION_ASSET_NAME;
		public const string DEFAULT_TROPHY_ASSET_NAME = "GJAPIDefaultTrophy";
		public const string DEFAULT_TROPHY_ASSET_PATH = IMAGE_RESOURCE_REL_PATH + DEFAULT_TROPHY_ASSET_NAME;
	}
}
