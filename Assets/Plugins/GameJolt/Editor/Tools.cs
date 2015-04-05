using UnityEngine;
using UnityEditor;

namespace GJAPI.Editor
{
	public class Tools : MonoBehaviour {
		
		[MenuItem("Edit/Project Settings/Game Jolt API")]
		public static void Settings()
		{
			var asset = AssetDatabase.LoadAssetAtPath (Constants.SETTINGS_ASSET_FULL_PATH, typeof(Settings)) as Settings;
			if (asset == null) {
				asset = ScriptableObject.CreateInstance<Settings>();
				AssetDatabase.CreateAsset (asset, Constants.SETTINGS_ASSET_FULL_PATH);
				AssetDatabase.SaveAssets ();
			}
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		[MenuItem("GameObject/Game Jolt API/Manager")]
		public static void Manager()
		{
			var manager = FindObjectOfType<Manager>();
			if (manager != null)
			{
				Selection.activeObject = manager;
			}
			else
			{
				var go = new GameObject(Constants.MANAGER_NAME);
				go.AddComponent<Manager>();
				Selection.activeObject = go;
			}
		}
	}
}