using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace GameJolt.Editor
{
	public class Tools : MonoBehaviour {
		
		[MenuItem("Edit/Project Settings/Game Jolt API")]
		public static void Settings()
		{
			var asset = AssetDatabase.LoadAssetAtPath(GameJolt.API.Constants.SETTINGS_ASSET_FULL_PATH, typeof(GameJolt.API.Settings)) as GameJolt.API.Settings;
			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<GameJolt.API.Settings>();
				AssetDatabase.CreateAsset (asset, GameJolt.API.Constants.SETTINGS_ASSET_FULL_PATH);
				AssetDatabase.SaveAssets ();
			}
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		[MenuItem("GameObject/Game Jolt API Manager")]
		public static void Manager()
		{
			var manager = FindObjectOfType<GameJolt.API.Manager>();
			if (manager != null)
			{
				Selection.activeObject = manager;
			}
			else
			{
				var prefab = AssetDatabase.LoadAssetAtPath(GameJolt.API.Constants.MANAGER_ASSET_FULL_PATH, typeof(GameObject)) as GameObject;
				if (prefab == null)
				{
					Debug.LogError("Unable to locate Game Jolt API prefab.");
				}
				else
				{
					var clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject; 
					Selection.activeObject = clone;

					if (FindObjectOfType<EventSystem>() == null)
					{
						new GameObject(
							"EventSystem",
							typeof(EventSystem),
							typeof(StandaloneInputModule),
							typeof(StandaloneInputModule)
						);
					}
				}
			}
		}
	}
}