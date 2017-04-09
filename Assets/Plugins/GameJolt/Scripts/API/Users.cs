using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJolt.API
{
	/// <summary>
	/// Users API methods
	/// </summary>
	public static class Users
	{
		#region Get
		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.User"/> by name.
		/// </summary>
		/// <param name="name">The <see cref="GameJolt.API.Objects.User"/> `Name`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="GameJolt.API.Objects.User"/>.</param>
		public static void Get(string name, Action<Objects.User> callback)
		{
			var user = new Objects.User(name, string.Empty);
			Get(user, callback);
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.User"/> by ID.
		/// </summary>
		/// <param name="name">The <see cref="GameJolt.API.Objects.User"/> `ID`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="GameJolt.API.Objects.User"/>.</param>
		public static void Get(int id, Action<Objects.User> callback)
		{
			var user = new Objects.User(id);
			Get(user, callback);
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.User"/> information.
		/// </summary>
		/// <param name="user">A <see cref="GameJolt.API.Objects.User"/> with either `Name` or `ID` set.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="GameJolt.API.Objects.User"/>.</param>
		public static void Get(Objects.User user, Action<Objects.User> callback)
		{
			var parameters = new Dictionary<string, string>();
			if(user.Name != null && user.Name != string.Empty)
			{
				parameters.Add("username", user.Name.ToLower());
			}
			else if (user.ID != 0)
			{
				parameters.Add("user_id", user.ID.ToString());
			}

			Core.Request.Get(Constants.API_USERS_FETCH, parameters, (Core.Response response) => {
				if(response.success)
				{
					user.BulkUpdate(response.json["users"][0].AsObject);
				}
				else
				{
					user = null;
				}

				if (callback != null)
				{
					callback(user);
				}
			}, false);
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.User"/>s by ID.
		/// </summary>
		/// <param name="user">An array of <see cref="GameJolt.API.Objects.User"/>s IDs</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.User"/>s.</param>
		public static void Get(int[] ids, Action<Objects.User[]> callback)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add(Constants.API_USERS_FETCH, string.Join(",", ids.Select(id => id.ToString()).ToArray()));

			Core.Request.Get("users/", parameters, (Core.Response response) => {
				Objects.User[] users;
				if(response.success)
				{
					int count = response.json["users"].AsArray.Count;
					users = new Objects.User[count];

					for (int i = 0; i < count; ++i)
					{
						users[i] = new Objects.User(response.json["users"][i].AsObject);
					}
				}
				else
				{
					users = null;
				}
				
				if (callback != null)
				{
					callback(users);
				}
			}, false);
		}
		#endregion Get
	}
}