using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Objects
{
	public class Notification
	{
		#region Fields & Properties
		public string Text { get; set; }
		public Sprite Image { get; set; }
		#endregion Fields & Properties
		
		#region Constructors
		public Notification(string text)
		{
			this.Text = text;

			var tex = Resources.Load(GameJolt.API.Constants.DEFAULT_NOTIFICATION_ASSET_PATH) as Texture2D;
			this.Image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), tex.width);
		}

		public Notification(string text, Sprite image)
		{
			this.Text = text;
			this.Image = image;
		}
		#endregion Constructors
	}
}
