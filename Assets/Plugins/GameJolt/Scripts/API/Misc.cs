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
			Manager.Instance.StartCoroutine(Manager.Instance.GetRequest(url, Core.ResponseFormat.Texture, (Core.Response response) => {
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
	}
}