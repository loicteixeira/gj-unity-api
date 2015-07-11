using UnityEngine;
using System;
using System.Collections.Generic;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	public enum UserType { Undefined, User, Developer, Moderator, Admin };
	public enum UserStatus { Undefined, Active, Banned };

	public class User : Base
	{
		#region Fields & Properties
		string name = "";
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

		public bool IsAuthenticated { get; private set; }

		public int ID { get; set; }

		public UserType Type { get; private set; }

		public UserStatus Status { get; private set; }

		public string AvatarURL { get; set; }

		public Sprite Avatar { get; set; }
		#endregion Fields & Properties

		#region Constructors
		public User(int id)
		{
			this.IsAuthenticated = false;

			this.ID = id;
		}

		public User(string name, string token)
		{
			this.IsAuthenticated = false;

			this.Name = name;
			this.Token = token;
		}

		public User(JSONClass data) : base()
		{
			this.IsAuthenticated = false;
			this.PopulateFromJSON(data);
		}
		#endregion Constructors

		#region Update Attributes
		protected override void PopulateFromJSON(JSONClass data)
		{
			this.Name = data["username"].Value;
			this.ID = data["id"].AsInt;
			this.AvatarURL = data["avatar_url"].Value;
			
			try
			{
				this.Type = (UserType)Enum.Parse(typeof(UserType), data["type"].Value);
			}
			catch
			{
				this.Type = UserType.Undefined;
			}

			try
			{
				this.Status = (UserStatus)Enum.Parse(typeof(UserStatus), data["status"].Value);
			}
			catch
			{
				this.Status = UserStatus.Undefined;
			}
		}
		#endregion Update Attributes

		#region Interface
		public void SignIn(Action<bool> callback = null)
		{
			if (Manager.Instance.CurrentUser != null)
			{
				Debug.LogWarning("Another user is currently signed in. Sign it out first.");

				if (callback != null)
				{
					callback(false);
				}

				return;
			}

			var parameters = new Dictionary<string, string>();
			parameters.Add("username", Name.ToLower());
			parameters.Add("user_token", Token.ToLower());

			Core.Request.Get(Constants.API_USERS_AUTH, parameters, (Core.Response response) => {
				IsAuthenticated = response.success;

				if (response.success)
				{
					Manager.Instance.CurrentUser = this;
					Get();
				}
				
				if (callback != null)
				{
					callback(response.success);
				}
			}, false);
		}

		public void SignOut()
		{
			if (Manager.Instance.CurrentUser == this)
			{
				Manager.Instance.CurrentUser = null;
			}
		}

		public void Get(Action<User> callback = null)
		{
			var me = this;
			Users.Get(me, callback);
		}

		public void DownloadAvatar(Action<bool> callback = null)
		{
			if (!string.IsNullOrEmpty(AvatarURL))
			{
				Misc.DownloadImage(AvatarURL, (Sprite avatar) => {
					if (avatar != null)
					{
						Avatar = avatar;
					}
					else
					{
						var tex = Resources.Load(GameJolt.API.Constants.DEFAULT_AVATAR_ASSET_PATH) as Texture2D;
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

		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.User: {0} - {1} - Authenticated: {2} - Status: {3}", Name, ID, IsAuthenticated, Status);
		}
	}
}
