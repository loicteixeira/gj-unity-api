using UnityEngine;

namespace GameJolt.API.Core
{
	/// <summary>
	/// Singleton utility for managers.
	/// </summary>
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null) 
				{
					instance = FindObjectOfType<T>();
					
					if (instance == null)
					{
						Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
					}
				}
				
				return instance;
			}
		}

		void Awake()
		{
			if (Persist())
			{
				Init();
			}
		}

		bool Persist()
		{
			if (instance == null)
			{
				instance = this as T;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
				return false;
			}

			// Only set DontDestroyOnLoad to top level objects.
			if (gameObject.transform.parent == null) {
				DontDestroyOnLoad (gameObject);
			}

			return true;
		}

		protected virtual void Init() {}
	}
}
