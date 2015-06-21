using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	public enum DataStoreOperation { Add, Subtract, Multiply, Divide, Append, Prepend };

	public static class DataStore
	{
		public static void Set(string key, string value, bool global, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("key", key);

			var payload = new Dictionary<string, string>();
			payload.Add("data", value);

			Core.Request.Post(Constants.API_DATASTORE_SET, parameters, payload, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, !global);
		}

		public static void Update(string key, string value, DataStoreOperation operation, bool global, Action<string> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("key", key);
			parameters.Add("operation", operation.ToString().ToLower());
			
			var payload = new Dictionary<string, string>();
			payload.Add("value", value);

			Core.Request.Post(Constants.API_DATASTORE_UPDATE, parameters, payload, (Core.Response response) => {
				if (callback != null)
				{
					if(response.success)
					{
						callback(response.dump);
					}
					else
					{
						callback(null);
					}
				}
			}, !global, Core.ResponseFormat.Dump);
		}

		public static void Get(string key, bool global, Action<string> callback)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("key", key);

			Core.Request.Get(Constants.API_DATASTORE_FETCH, parameters, (Core.Response response) => {
				string value;
				if (response.success)
				{
					value = response.dump;
				}
				else
				{
					value = null;
				}

				if (callback != null)
				{
					callback(value);
				}
			}, !global, Core.ResponseFormat.Dump);
		}

		public static void Delete(string key, bool global, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("key", key);

			Core.Request.Get(Constants.API_DATASTORE_REMOVE, parameters, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, !global);
		}

		public static void GetKeys(bool global, Action<string[]> callback)
		{
			Core.Request.Get(Constants.API_DATASTORE_KEYS_FETCH, null, (Core.Response response) => {
				string[] keys;
				if (response.success)
				{
					int count = response.json["keys"].AsArray.Count;
					keys = new string[count];

					for (int i = 0; i < count; ++i)
					{
						keys[i] = response.json["keys"][i]["key"].Value;
					}
				}
				else
				{
					keys = null;
				}

				if (callback != null)
				{
					callback(keys);
				}
			}, !global);
		}
	}
}