using UnityEngine;
using System;
using System.Collections.Generic;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	/// <summary>
	/// User types.
	/// </summary>
	public enum UserType { Undefined, User, Developer, Moderator, Admin };

	/// <summary>
	/// User statuses.
	/// </summary>
	public enum UserStatus { Undefined, Active, Banned };

	/// <summary>
	/// User objects.
	/// </summary>
	public sealed class User : Base
	{
		#region Fields & Properties
		string name = "";
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		/// <remarks>
		/// <para>
		/// Setting the name to a different value (case insensitive)
		/// will cause an authenticated <see cref="User"/> to not be authenticated anymore.
		/// </para>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Name
		{ 
			get { return name; }
			set
			{
				if (name.ToLower() != value.ToLower())
				{
					IsAuthenticated = false;
				}
				name = value;
			}
		}

		string token = "";
		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		/// <value>The token.</value>
		/// <remarks>
		/// <para>
		/// Setting the token to a different value (case insensitive)
		/// will cause an authenticated <see cref="User"/> to not be authenticated anymore.
		/// </para>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Token
		{ 
			get { return token; }
			set
			{
				if (token.ToLower() != value.ToLower())
				{
					IsAuthenticated = false;
				}
				token = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="User"/> is authenticated.
		/// </summary>
		/// <value><c>true</c> if this <see cref="User"/> is authenticated; otherwise, <c>false</c>.</value>
		public bool IsAuthenticated { get; private set; }

		/// <summary>
		/// Gets or sets the ID.
		/// </summary>
		/// <value>The ID.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public int ID { get; set; }

		/// <summary>
		/// Gets the user type.
		/// </summary>
		/// <value>The user type.</value>
		public UserType Type { get; private set; }

		/// <summary>
		/// Gets the user status.
		/// </summary>
		/// <value>The user status.</value>
		public UserStatus Status { get; private set; }

		/// <summary>
		/// Gets or sets the user avatar URL.
		/// </summary>
		/// <value>The user avatar URL.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string AvatarURL { get; set; }

		/// <summary>
		/// Gets or sets the user avatar.
		/// </summary>
		/// <value>The user avatar.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public Sprite Avatar { get; set; }
		#endregion Fields & Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="id">The <see cref="User"/> ID.</param>
		public User(int id)
		{
			IsAuthenticated = false;

			ID = id;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="name">The <see cref="User"/> name.</param>
		/// <param name="token">The <see cref="User"/> token.</param>
		public User(string name, string token)
		{
			IsAuthenticated = false;

			Name = name;
			Token = token;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="data">API JSON data.</param>
		public User(JSONClass data)
		{
			IsAuthenticated = false;
			PopulateFromJSON(data);
		}
		#endregion Constructors

		#region Update Attributes
		/// <summary>
		/// Map JSON data to the object's attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		protected override void PopulateFromJSON(JSONClass data)
		{
			Name = data["username"].Value;
			ID = data["id"].AsInt;
			AvatarURL = data["avatar_url"].Value;
			
			try
			{
				Type = (UserType)Enum.Parse(typeof(UserType), data["type"].Value);
			}
			catch
			{
				Type = UserType.Undefined;
			}

			try
			{
				Status = (UserStatus)Enum.Parse(typeof(UserStatus), data["status"].Value);
			}
			catch
			{
				Status = UserStatus.Undefined;
			}
		}
		#endregion Update Attributes

		#region Interface
		/// <summary>
		/// Signs in the user.
		/// </summary>
		/// <param name="signedInCallback">A callback function accepting a single parameter, a boolean indicating whether the user has been signed-in successfully.</param>
		/// <param name="userFetchedCallback">A callback function accepting a single parameter, a boolean indicating whether the user's information have been fetched successfully.</param>
		/// <param name="rememberMe">Whether the user's credentials should be stored in the player prefs.</param>
		public void SignIn(Action<bool> signedInCallback = null, Action<bool> userFetchedCallback = null, bool rememberMe = false)
		{
			if (Manager.Instance.CurrentUser != null)
			{
				Debug.LogWarning("Another user is currently signed in. Sign it out first.");

				if (signedInCallback != null)
				{
					signedInCallback(false);
				}
				if (userFetchedCallback != null)
				{
					userFetchedCallback(false);
				}

				return;
			}

			var parameters = new Dictionary<string, string> {{"username", Name.ToLower()}, {"user_token", Token.ToLower()}};
			Core.Request.Get(Constants.API_USERS_AUTH, parameters, response => {
				IsAuthenticated = response.success;

				if (response.success)
				{
					Manager.Instance.CurrentUser = this;

					if(rememberMe) {
						Manager.Instance.RememberUserCredentials(Name, Token);
					}

					if (signedInCallback != null)
					{
						signedInCallback(true);
					}

					Get((user) => {
						if (userFetchedCallback != null)
						{
							userFetchedCallback(user != null);
						}
					});
				}
				else
				{
					if (signedInCallback != null)
					{
						signedInCallback(false);
					}
					if (userFetchedCallback != null)
					{
						userFetchedCallback(false);
					}
				}
			}, false);
		}

		/// <summary>
		/// Signs out the user.
		/// </summary>
		public void SignOut()
		{
			if (Manager.Instance.CurrentUser == this)
			{
				Manager.Instance.CurrentUser = null;
				Manager.Instance.ClearUserCredentials();
			}
		}

		/// <summary>
		/// Get the <see cref="User"/> information.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="User"/>.</param>
		/// <remarks>
		/// <para>
		/// Shortcut for <c>GameJolt.API.Users.Get(this);</c>
		/// </para>
		/// </remarks>
		public void Get(Action<User> callback = null)
		{
			var me = this;
			Users.Get(me, callback);
		}

		/// <summary>
		/// Downloads the <see cref="User"/> avatar.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		/// <remarks>
		/// <para>
		/// Will set the `Avatar` field on the user. 
		/// </para>
		/// </remarks>
		public void DownloadAvatar(Action<bool> callback = null)
		{
			if (!string.IsNullOrEmpty(AvatarURL))
			{
				Misc.DownloadImage(AvatarURL, avatar => {
					if (avatar != null)
					{
						Avatar = avatar;
					}
					else
					{
						var tex = Resources.Load<Texture2D>(Constants.DEFAULT_AVATAR_ASSET_PATH);
						Avatar = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), tex.width);
					}

					if (callback != null)
					{
						callback(avatar != null);
					}
				});
			}
			else
			{
				if (callback != null)
				{
					callback(false);
				}
			}
		}
		#endregion Interface

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="User"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents the current <see cref="User"/>.</returns>
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.User: {0} - {1} - Authenticated: {2} - Status: {3}", Name, ID, IsAuthenticated, Status);
		}
	}
}
