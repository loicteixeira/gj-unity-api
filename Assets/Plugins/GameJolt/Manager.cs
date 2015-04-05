using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GJAPI
{
	public class Manager : MonoBehaviour
	{
		#region Singleton
		protected static Manager instance;
		public static Manager Instance
		{
			get
			{
				if (instance == null) 
				{
					instance = FindObjectOfType<Manager>();
					
					if (instance == null)
					{
						Debug.LogError("An instance of " + typeof(Manager) + " is needed in the scene, but there is none.");
					}
					else
					{
						DontDestroyOnLoad(instance.gameObject);
						instance.Configure();
					}
				}
				
				return instance;
			}
		}
		#endregion Singleton

		#region Fields & Properties
		public int GameID { get; private set; }
		public string PrivateKey { get; private set; }

		public float Timeout { get; set; }

		public Objects.User CurrentUser { get; set; }
		#endregion Fields & Properties

		#region Config
		protected void Configure()
		{
			var settings = Resources.Load(Constants.SETTINGS_ASSET_NAME) as Settings;
			GameID = settings.gameId;
			PrivateKey = settings.privateKey;
			Timeout = settings.timeout;

			if (GameID == 0)
			{
				Debug.LogWarning("Missing Game ID.");
			}
			if (PrivateKey == string.Empty)
			{
				Debug.LogWarning("Missing Private Key.");
			}

		}
		#endregion Config

		public IEnumerator GetRequest(string url, Core.ResponseFormat format, Action<Core.Response> callback)
		{
			float timeout = Time.time + Timeout;
			string error = null;

			var www = new WWW(url);
			while (!www.isDone)
			{
				if (Time.time > timeout)
				{
					error = "success:\"false\"\nmessage:\"Timeout\"";
					break;
				}
				yield return new WaitForEndOfFrame();
			}

			if (www.error != null)
			{
				error = "success:\"false\"\nmessage:\"" + www.error + "\"";
			}

			Core.Response response;
			if (error != null)
			{
				response = new Core.Response(error, Core.ResponseFormat.KeyPair);
			}
			else
			{
				response = new Core.Response(www.text, format);
			}
			callback(response);
		}

		public IEnumerator PostRequest(string url, Dictionary<string, string> payload, Core.ResponseFormat format, Action<Core.Response> callback)
		{
			var form = new WWWForm();
			foreach (KeyValuePair<string,string> field in payload)
			{
				form.AddField(field.Key, field.Value);
			}

			float timeout = Time.time + Timeout;
			string error = null;

			var www = new WWW (url, form);
			while (!www.isDone)
			{
				if (Time.time > timeout)
				{
					error = "success:\"false\"\nmessage:\"Timeout\"";
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			
			if (www.error != null)
			{
				error = "success:\"false\"\nmessage:\"" + www.error + "\"";
			}
			
			Core.Response response;
			if (error != null)
			{
				response = new Core.Response(error, Core.ResponseFormat.KeyPair);
			}
			else
			{
				response = new Core.Response(www.text, format);
			}
			callback(response);
		}
	}
}