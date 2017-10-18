using System;
using System.Collections.Generic;
using System.Linq;
using GameJolt.API.Objects;

namespace GameJolt.API
{
	/// <summary>
	/// Users API methods
	/// </summary>
	public static class Users
	{
		#region Get
		/// <summary>
		/// Get the <see cref="User"/> by name.
		/// </summary>
		/// <param name="name">The <see cref="User"/> `Name`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="User"/>.</param>
		public static void Get(string name, Action<User> callback)
		{
			var user = new User(name, string.Empty);
			Get(user, callback);
		}

		/// <summary>
		/// Get the <see cref="User"/> by ID.
		/// </summary>
		/// <param name="id">The <see cref="User"/> `ID`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="User"/>.</param>
		public static void Get(int id, Action<User> callback)
		{
			var user = new User(id);
			Get(user, callback);
		}

		/// <summary>
		/// Get the <see cref="User"/> information.
		/// </summary>
		/// <param name="user">A <see cref="User"/> with either `Name` or `ID` set.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="User"/>.</param>
		public static void Get(User user, Action<User> callback)
		{
			var parameters = new Dictionary<string, string>();
			if(!string.IsNullOrEmpty(user.Name))
			{
				parameters.Add("username", user.Name.ToLower());
			}
			else if (user.ID != 0)
			{
				parameters.Add("user_id", user.ID.ToString());
			}

			Core.Request.Get(Constants.API_USERS_FETCH, parameters, response => {
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
		/// Get the <see cref="User"/>s by ID.
		/// </summary>
		/// <param name="ids">An array of <see cref="User"/>s IDs</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="User"/>s.</param>
		public static void Get(int[] ids, Action<User[]> callback)
		{
			var parameters = new Dictionary<string, string> {{"user_id", string.Join(",", ids.Select(id => id.ToString()).ToArray())}};

			Core.Request.Get(Constants.API_USERS_FETCH, parameters, response => {
				User[] users;
				if(response.success)
				{
					int count = response.json["users"].AsArray.Count;
					users = new User[count];

					for (int i = 0; i < count; ++i)
					{
						users[i] = new User(response.json["users"][i].AsObject);
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