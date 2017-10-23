using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	/// <summary>
	/// DataStore operations.
	/// </summary>
	public enum DataStoreOperation { Add, Subtract, Multiply, Divide, Append, Prepend };

	/// <summary>
	/// The DataStore API methods.
	/// </summary>
	public static class DataStore
	{
		/// <summary>
		/// Save the key/value pair in the DataStore.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="value">The value to store.</param>
		/// <param name="global">A boolean indicating whether the key is global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Set(string key, string value, bool global, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string> {{"key", key}};
			var payload = new Dictionary<string, string> {{"data", value}};

			Core.Request.Post(Constants.API_DATASTORE_SET, parameters, payload, response => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, !global);
		}

		/// <summary>
		/// Update the value for a given key in the DataStore.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="value">The new value to operate with.</param>
		/// <param name="operation">The <see cref="DataStoreOperation"/> to perform.</param>
		/// <param name="global">A boolean indicating whether the key is global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="callback">A callback function accepting a single parameter, a the updated key value.</param>
		public static void Update(string key, string value, DataStoreOperation operation, bool global, Action<string> callback = null)
		{
			var parameters = new Dictionary<string, string> {{"key", key}, {"operation", operation.ToString().ToLower()}};
			var payload = new Dictionary<string, string> {{"value", value}};

			Core.Request.Post(Constants.API_DATASTORE_UPDATE, parameters, payload, response => {
				if (callback != null) {
					callback(response.success ? response.dump : null);
				}
			}, !global, Core.ResponseFormat.Dump);
		}

		/// <summary>
		/// Get the value for a given key from the DataStore.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="global">A boolean indicating whether the key is global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="callback">A callback function accepting a single parameter, the key value.</param>
		public static void Get(string key, bool global, Action<string> callback)
		{
			var parameters = new Dictionary<string, string> {{"key", key}};

			Core.Request.Get(Constants.API_DATASTORE_FETCH, parameters, response => {
				var value = response.success ? response.dump : null;
				if (callback != null)
				{
					callback(value);
				}
			}, !global, Core.ResponseFormat.Dump);
		}

		/// <summary>
		/// Delete the key from the DataStore.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="global">A boolean indicating whether the key is global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Delete(string key, bool global, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string> {{"key", key}};

			Core.Request.Get(Constants.API_DATASTORE_REMOVE, parameters, response => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, !global);
		}

		/// <summary>
		/// Gets the list of available keys in the DataStore.
		/// </summary>
		/// <param name="global">A boolean indicating whether the keys are global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="callback">A callback function accepting a single parameter, a list of key names.</param>
		public static void GetKeys(bool global, Action<string[]> callback)
		{
			GetKeys(global, null, callback);
		}

		/// <summary>
		/// Gets the list of available keys in the DataStore.
		/// </summary>
		/// <param name="global">A boolean indicating whether the keys are global (<c>true</c>) or private to the user (<c>false</c>).</param>
		/// <param name="pattern">Only keys matching this pattern will be returned. Placeholder character is *</param>
		/// <param name="callback">A callback function accepting a single parameter, a list of key names.</param>
		public static void GetKeys(bool global, string pattern, Action<string[]> callback)
		{
			var parameters = new Dictionary<string, string>();
			if(!string.IsNullOrEmpty(pattern))
				parameters.Add("pattern", pattern);
			Core.Request.Get(Constants.API_DATASTORE_KEYS_FETCH, parameters, response => {
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