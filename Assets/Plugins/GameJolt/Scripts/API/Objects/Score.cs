using System;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	public class Score : Base
	{
		#region Fields & Properties
		public int Value { get; set; }
		public string Text { get; set; }
		public string Extra { get; set; }
		public string Time { get; set; }

		public int UserID { get; set; }
		public string UserName { get; set; }
		public string GuestName { get; set; }
		public string PlayerName
		{
			get
			{
				return UserName != null && UserName != string.Empty ? UserName : GuestName;
			}
		}
		#endregion Fields & Properties
		
		#region Constructors
		public Score(int value, string text, string guestName = "", string extra = "")
		{
			this.Value = value;
			this.Text = text;
			this.GuestName = guestName;
			this.Extra = extra;
		}

		public Score(JSONClass data)
		{
			this.PopulateFromJSON(data);
		}
		#endregion Constructors
		
		#region Update Attributes
		protected override void PopulateFromJSON(JSONClass data)
		{
			this.Value = data["sort"].AsInt;
			this.Text = data["score"].Value;
			this.Extra = data["extra_data"].Value;
			this.Time = data["stored"].Value;

			this.UserID = data["user_id"].AsInt;
			this.UserName = data["user"].Value;
			this.GuestName = data["guest"].Value;
		}
		#endregion Update Attributes

		#region Interface
		public void Add(int table = 0, Action<bool> callback = null)
		{
			Scores.Add(this, table, callback);
		}
		#endregion Interface
		
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Score: {0} - {1}", PlayerName, Value);
		}
	}
}