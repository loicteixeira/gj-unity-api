using SimpleJSON;

namespace GJAPI.Objects
{
	public class Score : Base
	{
		#region Fields & Properties
		public int Value { get; set; }
		public string Text { get; set; }
		public string Extra { get; set; }
		public string Time { get; set; }

		public int UserId { get; set; }
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

			this.UserId = data["user_id"].AsInt;
			this.UserName = data["user"].Value;
			this.GuestName = data["guest"].Value;
		}
		#endregion Update Attributes
		
		public override string ToString()
		{
			return string.Format("GJAPI.Objects.Score: {0} - {1}", PlayerName, Value);
		}
	}
}