using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameJolt.API.Objects;
using GameJolt.External;

namespace GameJolt.API
{
	/// <summary>
	/// The Core API Manager.
	/// </summary>
	public class Manager : Core.MonoSingleton<Manager> {
		#region Fields & Properties
		private const string UserCredentialsPreferences = "GJ-API-User-Credentials";

		/// <summary>
		/// Gets the game ID.
		/// </summary>
		/// <value>The game ID.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public int GameID { get; private set; }

		/// <summary>
		/// Gets the game private key.
		/// </summary>
		/// <value>The game private key.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public string PrivateKey { get; private set; }

		/// <summary>
		/// Gets or sets the time in seconds before an API call should timeout and return failure.
		/// </summary>
		/// <value>The timeout.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public float Timeout { get; set; }

		/// <summary>
		/// Gets a value indicating whether it should sutomatically create and ping sessions once a user has been authentified.
		/// </summary>
		/// <value><c>true</c> to auto ping; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public bool AutoPing { get; private set; }

		/// <summary>
		/// Gets a value indicating whether High Score Tables and Trophies information should be cached for faster display.
		/// </summary>
		/// <value><c>true</c> to use caching; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public bool UseCaching { get; private set; }

		private string EncryptionKey { get; set; }


		private User currentUser;
		/// <summary>
		/// Gets or sets the current user.
		/// </summary>
		/// <value>The current user.</value>
		public User CurrentUser
		{
			get { return currentUser; }
			set
			{
				currentUser = value;

				if (currentUser != null)
				{
					if (currentUser.IsAuthenticated)
					{
						StartAutoPing();
						CacheTrophies();
					}
				}
				else
				{
					StopAutoPing();
				}
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Emulate auto connect in the editor, the same way it would happen with a WebGL build.
		/// </summary>
		/// <value><c>true</c> to auto connect in the editor; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public bool DebugAutoConnect { get; private set; }

		/// <summary>
		/// Gets the debug user name.
		/// </summary>
		/// <value>The debug user name to use for `DebugAutoConnect`.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public string DebugUser { get; private set; }

		/// <summary>
		/// Gets the debug user token.
		/// </summary>
		/// <value>The debug user token to use for `DebugAutoConnect`.</value>
		/// <remarks>
		/// <para>
		/// Set this in the API settings (Edit > Project Settings > GameJolt API).
		/// </para>
		/// </remarks>
		public string DebugToken { get; private set; }
#endif
		#endregion Fields & Properties

		#region Init
		/// <summary>
		/// Init this instance.
		/// </summary>
		protected override void Init()
		{
			Configure();
			AutoConnect();
			CacheTables();
		}

		/// <summary>
		/// Configure this instance.
		/// </summary>
		private void Configure()
		{
			var settings = Resources.Load(Constants.SETTINGS_ASSET_NAME) as Settings;
			if (settings != null)
			{
				GameID = settings.gameID;
				PrivateKey = settings.privateKey;
				Timeout = settings.timeout;
				AutoPing = settings.autoPing;
				UseCaching = settings.useCaching;
				EncryptionKey = settings.encryptionKey;
				
				if (GameID == 0)
				{
					Debug.LogWarning("Missing Game ID.");
				}
				if (PrivateKey == string.Empty)
				{
					Debug.LogWarning("Missing Private Key.");
				}
				
#if UNITY_EDITOR
				DebugAutoConnect = settings.autoConnect;
				DebugUser = settings.user;
				DebugToken = settings.token;
#endif
			}
			else
			{
				Debug.LogWarning("Could not load settings.");
			}
		}
		#endregion Init

		#region Requests
		public IEnumerator GetRequest(string url, Core.ResponseFormat format, Action<Core.Response> callback)
		{
			if (GameID == 0 || PrivateKey == null) {
				callback(new Core.Response("Bad Credentials"));
				yield break;
			}

			float timeout = Time.time + Timeout;
			var www = new WWW(url);
			while (!www.isDone)
			{
				if (Time.time > timeout)
				{
					callback(new Core.Response("Timeout for " + url));
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			callback(new Core.Response(www, format));
		}

		public IEnumerator PostRequest(string url, Dictionary<string, string> payload, Core.ResponseFormat format, Action<Core.Response> callback)
		{
			if (GameID == 0 || PrivateKey == null) {
				callback(new Core.Response("Bad Credentials"));
				yield break;
			}

			var form = new WWWForm();
			foreach (KeyValuePair<string,string> field in payload)
			{
				form.AddField(field.Key, field.Value);
			}

			float timeout = Time.time + Timeout;

			var www = new WWW (url, form);
			while (!www.isDone)
			{
				if (Time.time > timeout)
				{
					callback(new Core.Response("Timeout for " + url));
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}

			callback(new Core.Response(www, format));
		}
		#endregion Requests

		#region Actions
		private void AutoConnect()
		{
#if UNITY_WEBPLAYER || UNITY_WEBGL
			#region Autoconnect Web
	#if UNITY_EDITOR
			if (DebugAutoConnect)
			{
				if (DebugUser != string.Empty && DebugToken != string.Empty)
				{
					var user = new Objects.User(DebugUser, DebugToken);
					user.SignIn(success => { Debug.Log(string.Format("AutoConnect: " + success)); });
				}
				else
				{
					Debug.LogWarning("Cannot simulate WebPlayer AutoConnect. Missing user and/or token in debug settings.");
				}
			}
	#else
			var uri = new Uri(Application.absoluteURL);
			if (uri.Host.EndsWith("gamejolt.net") || uri.Host.EndsWith("gamejolt.com"))
			{
				#if UNITY_WEBPLAYER
				Application.ExternalCall("GJAPI_AuthUser", this.gameObject.name, "OnAutoConnectWebPlayer");
				#elif UNITY_WEBGL
				Application.ExternalEval(string.Format(@"
var qs = location.search;
var params = {{}};
var tokens;
var re = /[?&]?([^=]+)=([^&]*)/g;

while (tokens = re.exec(qs)) {{
	params[decodeURIComponent(tokens[1])] = decodeURIComponent(tokens[2]);
}}

var message;
if ('gjapi_username' in params && params.gjapi_username !== '' && 'gjapi_token' in params && params.gjapi_token !== '') {{
	message = params.gjapi_username + ':' + params.gjapi_token;	
}}
else {{
	message = '';
}}

SendMessage('{0}', 'OnAutoConnectWebPlayer', message);
		", this.gameObject.name));
				#endif
			}
			else
			{
				Debug.Log("Cannot AutoConnect, the game is not hosted on GameJolt.");
			}
	#endif
			
			#endregion
#else
			#region Autoconnect Non Web
			string username, token;
			if(GetStoredUserCredentials(out username, out token)) {
				var user = new User(username, token);
				user.SignIn();
			}
			#endregion

#endif
		}

#if UNITY_WEBPLAYER || UNITY_WEBGL
		public void OnAutoConnectWebPlayer(string response)
		{
			if (response != string.Empty)
			{
				var credentials = response.Split(new[] {':'}, 2);
				if (credentials.Length == 2)
				{
					var user = new Objects.User(credentials[0], credentials[1]);
					user.SignIn();
					// TODO: Prompt "Welcome Back <username>!"
				}
				else
				{
					Debug.Log("Cannot AutoConnect.");
				}
			}
			else
			{
				// This is a Guest.
				// TODO: Prompt "Hello Guest!" and encourage to signup/signin?
			}
		}
#endif

		private void StartAutoPing()
		{
			if (!AutoPing)
			{
				return;
			}

			Sessions.Open(success => {
				// What should we do if it fails? Retry later?
				// Without smart handling, it will probably just fail again...
				if (success)
				{
					Invoke("Ping", 30f);
				}
			});
		}

		private void Ping()
		{
			Sessions.Ping(SessionStatus.Active, success => {
				// Sessions are automatically closed after 120 seconds
				// which will happen if the application has been in the background for too long.
				// It would be nice to Ping an Idle state when the app is in the background,
				// but because Unity apps don't run in the background by default, this is doomed to failure.
				// Let it error out and reconnect.
				if (!success)
				{
					Invoke("StartAutoPing", 1f); // Try reconnecting.
				}
				else
				{
					Invoke("Ping", 30f); // Ping again.
				}
			});
		}

		private void StopAutoPing()
		{
			if (AutoPing)
			{
				CancelInvoke("StartAutoPing");
				CancelInvoke("Ping");
			}
		}

		private void CacheTables()
		{
			if (UseCaching)
			{
				Scores.GetTables(null);
			}
		}

		private void CacheTrophies()
		{
			if (UseCaching)
			{
				Trophies.Get(trophies => {
					if (trophies != null)
					{
						foreach(Trophy trophy in trophies)
						{
							trophy.DownloadImage();
						}
					}
				});
			}
		}
#endregion Actions

		#region Helper
		/// <summary>
		/// If the user's credentials are stored in PlayerPrefs, this method will retrieve them.
		/// </summary>
		/// <param name="username">Contains the username if retrieval was successfull, empty string otherwise.</param>
		/// <param name="token">Contains the token if retrieval was successfull, empty string otherwise.</param>
		/// <returns>Whether retrieval was successfull or not.</returns>
		public bool GetStoredUserCredentials(out string username, out string token) {
			username = token = "";
			if(string.IsNullOrEmpty(UserCredentialsPreferences) || string.IsNullOrEmpty(EncryptionKey) ||
			   !PlayerPrefs.HasKey(UserCredentialsPreferences)) return false;
			var credentials = PlayerPrefs.GetString(UserCredentialsPreferences).Split('#');
			if(credentials.Length != 2) return false;
			try {
				username = XTEA.Decrypt(credentials[0], EncryptionKey);
				token = XTEA.Decrypt(credentials[1], EncryptionKey);
				return true;
			} catch {
				Debug.LogWarning("Failed to retrieve user credentials.");
				return false;
			}
		}

		/// <summary>
		/// Stores the user's credentials in PlayerPrefs.
		/// </summary>
		/// <param name="username">The username to store.</param>
		/// <param name="token">The token to store.</param>
		/// <returns>Whether the operations was successfull.</returns>
		public bool RememberUserCredentials(string username, string token) {
			if(string.IsNullOrEmpty(UserCredentialsPreferences) || string.IsNullOrEmpty(EncryptionKey) ||
			   string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token)) return false;
			var credentials = XTEA.Encrypt(username, EncryptionKey) + "#" + XTEA.Encrypt(token, EncryptionKey);
			PlayerPrefs.SetString(UserCredentialsPreferences, credentials);
			PlayerPrefs.Save();
			return true;
		}

		/// <summary>
		/// Clears the stored credentials.
		/// </summary>
		public void ClearUserCredentials() {
			if(string.IsNullOrEmpty(UserCredentialsPreferences)) return;
			PlayerPrefs.DeleteKey(UserCredentialsPreferences);
			PlayerPrefs.Save();
		}
		#endregion
	}
}