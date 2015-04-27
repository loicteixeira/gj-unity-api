using System;
using GJAPI.External.SimpleJSON;

namespace GJAPI.Objects
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
		#endregion Interface
		
		public override string ToString()
		{
			return string.Format("GJAPI.Objects.Trophy: {0} - {1} - {2} - {3}Unlocked", Title, ID, Difficulty, Unlocked ? "" : "Not ");
		}
	}
}