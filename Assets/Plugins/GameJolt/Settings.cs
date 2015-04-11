using UnityEngine;

namespace GJAPI
{
	[System.Serializable]
	public class Settings : ScriptableObject {
		[Header("Game")]
		public int gameId;
		public string privateKey;

		[Header("Settings")]
		public float timeout = 10f;
		public bool autoPing = true;

		[Header("Debug")]
		public bool autoConnect = false;
		public string user;
		public string token;
	}
}