using System;
using System.Collections.Generic;
using GJAPI.External.SimpleJSON;

namespace GJAPI.Objects
{
	public enum UserType { Undefined, User, Developer, Moderator, Admin };
	public enum UserStatus { Undefined, Active, Banned };

	public class User : Base
	{
		#region Fields & Properties
		string name;
		public string Name
		{ 
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					IsAuthenticated = false;
				}
			}
		}

		string token;
		public string Token
		{ 
			get { return token; }
			set
			{
				if (token != value)
				{
					token = value;
					IsAuthenticated = false;
				}
			}
		}

		public bool IsAuthenticated { get; set; }

		public int ID { get; set; }

		public UserType Type { get; private set; }

		public UserStatus Status { get; private set; }

		public string AvatarURL { get; set; }
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
		public void Authenticate(Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("username", Name);
			parameters.Add("user_token", Token);

			Core.Request.Get(Constants.API_USERS_AUTH, parameters, (Core.Response response) => {
				IsAuthenticated = response.success;

				if (response.success)
				{
					Manager.Instance.StartAutoPing();
				}
				
				if (callback != null)
				{
					callback(response.success);
				}
			}, false);
		}

		public void Get(Action<User> callback = null)
		{
			var me = this;
			Users.Get(me, callback);
		}
		#endregion Interface

		public override string ToString()
		{
			return string.Format("GJAPI.Objects.User: {0} - {1} - Authenticated: {2} - Status: {4}", Name, ID, IsAuthenticated, Status);
		}
	}
}
