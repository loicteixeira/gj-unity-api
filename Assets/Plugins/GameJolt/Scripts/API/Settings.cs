using UnityEngine;

namespace GameJolt.API
{
	[System.Serializable]
	public class Settings : ScriptableObject {
		[Header("Game")]
		public int gameId;
		public string privateKey;

		[Header("Settings")]
		public float timeout = 10f;
		public bool autoPing = true;
		public bool useCaching = true;

		[Header("Debug")]
		public bool autoConnect = false;
		public string user;
		public string token;
	}
}