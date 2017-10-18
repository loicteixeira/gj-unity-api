using System;
using System.Collections.Generic;
using System.Linq;
using GameJolt.API.Objects;

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
		static Dictionary<int, Trophy> cachedTrophies;

		#region Unlock
		/// <summary>
		/// Unlock the specified <see cref="Trophy"/>.
		/// </summary>
		/// <param name="trophy">The <see cref="Trophy"/> to unlock.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Unlock(Trophy trophy, Action<bool> callback = null)
		{
			Unlock(trophy.ID, callback);
		}

		/// <summary>
		/// Unlock the specified <see cref="Trophy"/> by its id.
		/// </summary>
		/// <param name="id">The <see cref="Trophy"/> ID.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Unlock(int id, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string> {{"trophy_id", id.ToString()}};

			Core.Request.Get(Constants.API_TROPHIES_ADD, parameters, response => {
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
		/// Get the <see cref="Trophy"/> image from cache or download it before displaying a notification.
		/// </summary>
		/// <param name="id">The <see cref="Trophy"/> `id`.</param>
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
					cachedTrophies[id].DownloadImage(success => {
						ShowNotification(cachedTrophies[id]);
					});
				}
			}
			else
			{
				Get (id, trophy => {
					if (trophy != null)
					{
						trophy.DownloadImage(success => {
							ShowNotification(trophy);
						});
					}
				});
			}
		}

		/// <summary>
		/// Shows a <see cref="Trophy"/> unlock notification.
		/// </summary>
		/// <param name="trophy">The <see cref="Trophy"/> to display in the notification.</param>
		static void ShowNotification(Trophy trophy)
		{
			if (trophy.Unlocked)
			{
				UI.Manager.Instance.QueueNotification(
					string.Format("Unlocked <b>#{0}</b>", trophy.Title),
					trophy.Image);
			}
		}
		#endregion Unlock

		#region Get
		/// <summary>
		/// Get a <see cref="Trophy"/> by `id`.
		/// </summary>
		/// <param name="id">The <see cref="Trophy"/> `id`.</param>
		/// <param name="callback">A callback function accepting a single parameter, a <see cref="Trophy"/>.</param>
		public static void Get(int id, Action<Trophy> callback)
		{
			if (cachedTrophies != null)
			{
				if (callback != null)
				{
					callback(cachedTrophies.Values.FirstOrDefault(t => t.ID == id));
				}
			}
			else
			{
				var parameters = new Dictionary<string, string> {{"trophy_id", id.ToString()}};

				Core.Request.Get(Constants.API_TROPHIES_FETCH, parameters, response => {
					var trophy = response.success ? new Trophy(response.json["trophies"][0].AsObject) : null;

					if (callback != null)
					{
						callback(trophy);
					}
				});
			}
		}

		/// <summary>
		/// Get the <see cref="Trophy"/>s by id.
		/// </summary>
		/// <param name="ids">An array of <see cref="Trophy"/>s IDs</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Trophy"/>s.</param>
		public static void Get(int[] ids, Action<Trophy[]> callback)
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
				var parameters = new Dictionary<string, string> {{"trophy_id", string.Join(",", ids.Select(id => id.ToString()).ToArray())}};

				Get(parameters, callback);
			}
		}

		/// <summary>
		/// Get the <see cref="Trophy"/>s information.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Trophy"/>s.</param>
		public static void Get(Action<Trophy[]> callback)
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

				Get(parameters, trophies => {
					
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
		/// Get all locked/unlocked <see cref="Trophy"/>s.
		/// </summary>
		/// <param name="unlocked">A boolean indicating whether to retrieve unlocked (<c>true</c>) or locked (<c>false</c>) <see cref="Trophy"/>s.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Trophy"/>s.</param>
		public static void Get(bool unlocked, Action<Trophy[]> callback)
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
				var parameters = new Dictionary<string, string> {{"achieved", unlocked.ToString().ToLower()}};

				Get(parameters, callback);
			}
		}

		/// <summary>
		/// Get <see cref="Trophy"/>s.
		/// </summary>
		/// <param name="parameters">The API call parameters.</param>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="Trophy"/>s.</param>
		static void Get(Dictionary<string, string> parameters, Action<Trophy[]> callback)
		{
			Core.Request.Get(Constants.API_TROPHIES_FETCH, parameters, response => {
				Trophy[] trophies;
				if (response.success)
				{
					int count = response.json["trophies"].AsArray.Count;
					trophies = new Trophy[count];
					
					for(int i = 0; i < count; ++i)
					{
						trophies[i] = new Trophy(response.json["trophies"][i].AsObject);
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