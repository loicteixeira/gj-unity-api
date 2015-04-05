using System;
using SimpleJSON;

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
		public string ImageUrl { get; set; }
		#endregion Fields & Properties
		
		#region Constructors
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
			this.ImageUrl = data["image_url"].Value;
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
		
		public override string ToString()
		{
			return string.Format("GJAPI.Objects.Trophy: {0} - {1} - {2} - {3}Unlocked", Title, ID, Difficulty, Unlocked ? "" : "Not ");
		}
	}
}