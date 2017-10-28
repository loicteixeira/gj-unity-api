using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using GameJolt.API;

namespace GameJolt.Editor
{
	public class Tools : MonoBehaviour {
		
		[MenuItem("Edit/Project Settings/Game Jolt API")]
		public static void Settings()
		{
			var asset = AssetDatabase.LoadAssetAtPath(Constants.SETTINGS_ASSET_FULL_PATH, typeof(Settings)) as Settings;
			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<Settings>();
				AssetDatabase.CreateAsset(asset, Constants.SETTINGS_ASSET_FULL_PATH);
				AssetDatabase.SaveAssets();
			}
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		[MenuItem("GameObject/Game Jolt API Manager")]
		public static void Manager()
		{
			var manager = FindObjectOfType<Manager>();
			if (manager != null)
			{
				Selection.activeObject = manager;
			}
			else
			{
				var prefab = AssetDatabase.LoadAssetAtPath(Constants.MANAGER_ASSET_FULL_PATH, typeof(GameObject)) as GameObject;
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