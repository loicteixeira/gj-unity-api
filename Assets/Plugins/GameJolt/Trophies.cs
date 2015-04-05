using System;
using System.Collections.Generic;

namespace GJAPI
{
	public static class Trophies
	{
		#region Unlock
		public static void Unlock(int id, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("trophy_id", id.ToString());

			Core.Request.Get(Constants.API_TROPHIES_ADD, parameters, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			});
		}
		#endregion Unlock

		#region Get
		public static void Get(int id, Action<Objects.Trophy> callback)
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

		public static void Get(int[] ids, Action<Objects.Trophy[]> callback)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("trophy_id", string.Join(",", Array.ConvertAll(ids, t => t.ToString())));

			Get(parameters, callback);
		}

		public static void Get(Action<Objects.Trophy[]> callback)
		{
			var parameters = new Dictionary<string, string>();

			Get(parameters, callback);
		}

		public static void Get(bool unlocked, Action<Objects.Trophy[]> callback)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("achieved", unlocked.ToString().ToLower());

			Get(parameters, callback);
		}

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