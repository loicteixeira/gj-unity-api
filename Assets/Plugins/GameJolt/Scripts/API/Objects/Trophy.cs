using UnityEngine;
using System;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	/// <summary>
	/// Trophy difficulties.
	/// </summary>
	public enum TrophyDifficulty { Undefined, Bronze, Silver, Gold, Platinum }

	/// <summary>
	/// Trophy object.
	/// </summary>
	public class Trophy : Base
	{
		#region Fields & Properties
		/// <summary>
		/// Gets or sets the ID.
		/// </summary>
		/// <value>The ID.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the difficulty.
		/// </summary>
		/// <value>The difficulty.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public TrophyDifficulty Difficulty { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="GameJolt.API.Objects.Trophy"/> is unlocked.
		/// </summary>
		/// <value><c>true</c> if unlocked; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public bool Unlocked { get; set; }

		/// <summary>
		/// Gets or sets the image UR.
		/// </summary>
		/// <value>The image UR.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string ImageURL { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public Sprite Image { get; set; }
		#endregion Fields & Properties
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Objects.Trophy"/> class.
		/// </summary>
		/// <param name="id">The <see cref="GameJolt.API.Objects.Trophy"/> ID.</param>
		/// <param name="title">The <see cref="GameJolt.API.Objects.Trophy"/> title.</param>
		/// <param name="description">The <see cref="GameJolt.API.Objects.Trophy"/> description.</param>
		/// <param name="difficulty">The <see cref="GameJolt.API.Objects.Trophy"/> difficulty.</param>
		/// <param name="unlocked">If set to <c>true</c> unlocked.</param>
		public Trophy(int id, string title, string description, TrophyDifficulty difficulty, bool unlocked)
		{
			this.ID = id;
			this.Title = title;
			this.Description = description;
			this.Difficulty = difficulty;
			this.Unlocked = unlocked;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Objects.Trophy"/> class.
		/// </summary>
		/// <param name="data">API JSON data.</param>
		public Trophy(JSONClass data)
		{
			this.PopulateFromJSON(data);
		}
		#endregion Constructors
		
		#region Update Attributes
		/// <summary>
		/// Map JSON data to the object's attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		protected override void PopulateFromJSON(JSONClass data)
		{
			this.ID = data["id"].AsInt;
			this.Title = data["title"].Value;
			this.Description = data["description"].Value;
			this.ImageURL = data["image_url"].Value;
			this.Unlocked = data["achieved"].Value != "false";

			try
			{
				this.Difficulty = (TrophyDifficulty)Enum.Parse(typeof(TrophyDifficulty), data["difficulty"].Value);
			}
			catch
			{
				this.Difficulty = TrophyDifficulty.Undefined;
			}
		}
		#endregion Update Attributes

		#region Interface
		/// <summary>
		/// Unlock the <see cref="GameJolt.API.Objects.Trophy"/>.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		/// <remarks>
		/// <para>
		/// Shortcut for <c>GameJolt.API.Trophies.Unlock(this);</c>
		/// </para>
		/// </remarks>
		public void Unlock(Action<bool> callback = null)
		{
			Trophies.Unlock(this, (bool success) => {
				Unlocked = success;

				if (callback != null)
				{
					callback(success);
				}
			});
		}

		/// <summary>
		/// Downloads the <see cref="GameJolt.API.Objects.Trophy"/> image.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		/// <remarks>
		/// <para>
		/// Will set the `Image` field on the trophy. 
		/// </para>
		/// </remarks>
		public void DownloadImage(Action<bool> callback = null)
		{
			if (!string.IsNullOrEmpty(ImageURL))
			{
				Misc.DownloadImage(ImageURL, (Sprite image) => {
					if (image != null)
					{
						Image = image;
					}
					else
					{
						var tex = Resources.Load(GameJolt.API.Constants.DEFAULT_TROPHY_ASSET_PATH) as Texture2D;
						Image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), tex.width);
					}
					
					if (callback != null)
					{
						callback(image != null);
					}
				});
			}
			else
			{
				if (callback != null)
				{
					callback(false);
				}
			}
		}
		#endregion Interface

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="GameJolt.API.Objects.Trophy"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="GameJolt.API.Objects.Trophy"/>.</returns>
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Trophy: {0} - {1} - {2} - {3}Unlocked", Title, ID, Difficulty, Unlocked ? "" : "Not ");
		}
	}
}