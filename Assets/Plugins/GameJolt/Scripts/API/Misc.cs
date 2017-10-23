using UnityEngine;
using System;

namespace GameJolt.API
{
	/// <summary>
	/// Misc API methods.
	/// </summary>
	public static class Misc
	{
		/// <summary>
		/// Downloads an image.
		/// </summary>
		/// <param name="url">The image URL.</param>
		/// <param name="callback">A callback function accepting a single parameter, a UnityEngine.Sprite.</param>
		public static void DownloadImage(string url, Action<Sprite> callback)
		{
			Manager.Instance.StartCoroutine(Manager.Instance.GetRequest(url, Core.ResponseFormat.Texture, response => {
				Sprite sprite;
				if (response.success)
				{
					sprite = Sprite.Create(
						response.texture,
						new Rect(0, 0, response.texture.width, response.texture.height),
						new Vector2(.5f, .5f),
						response.texture.width);
				}
				else
				{
					sprite = null;
				}

				if (callback != null )
				{
					callback(sprite);
				}
			}));
		}

		/// <summary>
		/// Get the server time.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a System.DateTime.</param>
		public static void GetTime(Action<DateTime> callback)
		{
			Core.Request.Get(Constants.API_TIME_GET, null, response => {
				if (callback != null)
				{
					double timestamp = response.json["timestamp"].AsDouble;
					DateTime serverTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
					serverTime = serverTime.AddSeconds(timestamp);
					callback(serverTime);
				}
			}, false);
		}
	}
}