using UnityEngine;
using System;

namespace GameJolt.API
{
	public static class Misc
	{
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