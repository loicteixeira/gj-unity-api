using UnityEngine;
using System;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	public enum TrophyDifficulty { Undefined, Bronze, Silver, Gold, Platinum }

	public class Trophy : Base
	{
		#region Fields & Properties
		public int ID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public TrophyDifficulty Difficulty { get; set; }
		public bool Unlocked { get; set; }
		public string ImageURL { get; set; }
		public Sprite Image { get; set; }
		#endregion Fields & Properties
		
		#region Constructors
		public Trophy(int id, string title, string description, TrophyDifficulty difficulty, bool unlocked)
		{
			this.ID = id;
			this.Title = title;
			this.Description = description;
			this.Difficulty = difficulty;
			this.Unlocked = unlocked;
		}

		public Trophy(JSONClass data)
		{
			this.PopulateFromJSON(data);
		}
		#endregion Constructors
		
		#region Update Attributes
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
		
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Trophy: {0} - {1} - {2} - {3}Unlocked", Title, ID, Difficulty, Unlocked ? "" : "Not ");
		}
	}
}