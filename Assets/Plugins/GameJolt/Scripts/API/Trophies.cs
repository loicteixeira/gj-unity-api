using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJolt.API
{
	/// <summary>
	/// Trophies API methods
	/// </summary>
	public static class Trophies
	{
		/// <summary>
		/// The cached trophies.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Trophies will only be put in cache when calling the Get with no other parameter than the callback.
		/// However, any Get call will use the cache to retrieve information.
		/// Indeed, if any Get call could put stuff in the cache, when later asking for all the trophies,
		/// it would be impossible to know if we already cached them all or not.
		/// </para>
		/// <para>
		/// Trophies technically belongs to a user but then,
		/// this script would need to access the current user on the manager and everything would be too tangled.
		/// </para>
		/// </remarks>
		static Dictionary<int, Objects.Trophy> cachedTrophies = null;

		#region Unlock
		/// <summary>
		/// Unlock the specified <see cref="GameJolt.API.Objects.Trophy"/>.
		/// </summary>
		/// <param name="trophy">The <see cref="GameJolt.API.Objects.Trophy"/> to unlock.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Unlock(Objects.Trophy trophy, Action<bool> callback = null)
		{
			Unlock(trophy.ID, callback);
		}

		/// <summary>
		/// Unlock a <see The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table)..Trophy"/> by `ID`.
		/// </summary>
		/// <param name="trophy">The <see cref="GameJolt.API.Objects.Trophy"/> ID.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Unlock(int id, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("trophy_id", id.ToString());

			Core.Request.Get(Constants.API_TROPHIES_ADD, parameters, (Core.Response response) => {
				// Update the cache.
				if (cachedTrophies != null && cachedTrophies.ContainsKey(id) && !cachedTrophies[id].Unlocked)
				{
					cachedTrophies[id].Unlocked = response.success;
				}

				// Show the notification
				PrepareNotification(id);

				if (callback != null)
				{
					callback(response.success);
				}
			});
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.Trophy"/> image from cache or download it before displaying a notification.
		/// </summary>
		/// <param name="id">The <see cref="GameJolt.API.Objects.Trophy"/> `id`.</param>
		static void PrepareNotification(int id)
		{
			if (cachedTrophies != null && cachedTrophies.ContainsKey(id))
			{
				if (cachedTrophies[id].Image != null)
				{
					ShowNotification(cachedTrophies[id]);
				}
				else
				{
					cachedTrophies[id].DownloadImage((bool success) => {
						ShowNotification(cachedTrophies[id]);
					});
				}
			}
			else
			{
				Get (id, (Objects.Trophy trophy) => {
					if (trophy != null)
					{
						trophy.DownloadImage((bool success) => {
							ShowNotification(trophy);
						});
					}
				});
			}
		}

		/// <summary>
		/// Shows a <see cref="GameJolt.API.Objects.Trophy"/> unlock notification.
		/// </summary>
		/// <param name="trophy">The <see cref="GameJolt.API.Objects.Trophy"/> to display in the notification.</param>
		static void ShowNotification(Objects.Trophy trophy)
		{
			if (trophy.Unlocked)
			{
				GameJolt.UI.Manager.Instance.QueueNotification(
					string.Format("Unlocked <b>#{0}</b>", trophy.Title),
					trophy.Image);
			}
		}
		#endregion Unlock

		#region Get
		/// <summary>
		/// Get a <see cref="GameJolt.API.Objects.Trophy"/> by `id`.
		/// </summary>
		/// <param name="id">The <see cref="GameJolt.API.Objects.Trophy"/> `id`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="GameJolt.API.Objects.Trophy"/>.</param>
		public static void Get(int id, Action<Objects.Trophy> callback)
		{
			if (cachedTrophies != null)
			{
				if (callback != null)
				{
					callback(cachedTrophies.Values.Where(t => t.ID == id).FirstOrDefault());
				}
			}
			else
			{
				var parameters = new Dictionary<string, string>();
				parameters.Add("trophy_id", id.ToString());

				Core.Request.Get(Constants.API_TROPHIES_FETCH, parameters, (Core.Response response) => {
					Objects.Trophy trophy;
					if (response.success)
					{
						trophy = new Objects.Trophy(response.json["trophies"][0].AsObject);
					}
					else
					{
						trophy = null;
					}

					if (callback != null)
					{
						callback(trophy);
					}
				});
			}
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.Trophy"/>s by id.
		/// </summary>
		/// <param name="ids">An array of <see cref="GameJolt.API.Objects.Trophy"/>s IDs</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.Trophy"/>s.</param>
		public static void Get(int[] ids, Action<Objects.Trophy[]> callback)
		{
			if (cachedTrophies != null)
			{
				if (callback != null)
				{
					var trophies = cachedTrophies.Values.Where(t => ids.Contains(t.ID)).ToArray();
					callback(trophies.Length != 0 ? trophies : null);
				}
			}
			else
			{
				var parameters = new Dictionary<string, string>();
				parameters.Add("trophy_id", string.Join(",", ids.Select(id => id.ToString()).ToArray()));

				Get(parameters, callback);
			}
		}

		/// <summary>
		/// Get the <see cref="GameJolt.API.Objects.Trophy"/>s information.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.Trophy"/>s.</param>
		public static void Get(Action<Objects.Trophy[]> callback)
		{
			if (cachedTrophies != null)
			{
				if (callback != null)
				{
					callback(cachedTrophies.Values.ToArray());
				}
			}
			else
			{
				// There are no parameters but it cannot be left null
				// because the call to Get(Dictionary<string, string> parameters, Action<Objects.Trophy[]> callback)
				// would be ambiguous with  Get(int[] ids, Action<Objects.Trophy[]> callback)
				var parameters = new Dictionary<string, string>();

				Get(parameters, (Objects.Trophy[] trophies) => {
					
					if (Manager.Instance.UseCaching && trophies != null)
					{
						cachedTrophies = trophies.ToDictionary(t => t.ID, t => t);
					}
					
					if (callback != null)
					{
						callback(trophies);
					}
				});
			}
		}

		/// <summary>
		/// Get all locked/unlocked <see cref="GameJolt.API.Objects.Trophy"/>s.
		/// </summary>
		/// <param name="unlocked">A boolean indicating whether to retrieve unlocked (<c>true</c>) or locked (<c>false</c>) <see cref="GameJolt.API.Objects.Trophy"/>s.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.Trophy"/>s.</param>
		public static void Get(bool unlocked, Action<Objects.Trophy[]> callback)
		{
			if (cachedTrophies != null)
			{
				if (callback != null)
				{
					var trophies = cachedTrophies.Values.Where(t => t.Unlocked == unlocked).ToArray();
					callback(trophies.Length != 0 ? trophies : null);
				}
			}
			else
			{
				var parameters = new Dictionary<string, string>();
				parameters.Add("achieved", unlocked.ToString().ToLower());

				Get(parameters, callback);
			}
		}

		/// <summary>
		/// Get <see cref="GameJolt.API.Objects.Trophy"/>s.
		/// </summary>
		/// <param name="parameters">The API call parameters.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.Trophy"/>s.</param>
		static void Get(Dictionary<string, string> parameters, Action<Objects.Trophy[]> callback)
		{
			Core.Request.Get(Constants.API_TROPHIES_FETCH, parameters, (Core.Response response) => {
				Objects.Trophy[] trophies;
				if (response.success)
				{
					int count = response.json["trophies"].AsArray.Count;
					trophies = new Objects.Trophy[count];
					
					for(int i = 0; i < count; ++i)
					{
						trophies[i] = new Objects.Trophy(response.json["trophies"][i].AsObject);
					}
				}
				else
				{
					trophies = null;
				}
				
				if (callback != null)
				{
					callback(trophies);
				}
			});
		}
		#endregion Get
	}
}