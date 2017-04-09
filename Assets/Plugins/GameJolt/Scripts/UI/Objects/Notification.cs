using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Objects
{
	/// <summary>
	/// A UI Notification.
	/// </summary>
	public class Notification
	{
		#region Fields & Properties
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		public Sprite Image { get; set; }
		#endregion Fields & Properties
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.UI.Objects.Notification"/> class.
		/// </summary>
		/// <param name="text">The notification text.</param>
		public Notification(string text)
		{
			this.Text = text;

			var tex = Resources.Load(GameJolt.API.Constants.DEFAULT_NOTIFICATION_ASSET_PATH) as Texture2D;
			this.Image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), tex.width);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.UI.Objects.Notification"/> class.
		/// </summary>
		/// <param name="text">The notification text.</param>
		/// <param name="image">The notification image.</param>
		public Notification(string text, Sprite image)
		{
			this.Text = text;
			this.Image = image;
		}
		#endregion Constructors
	}
}
