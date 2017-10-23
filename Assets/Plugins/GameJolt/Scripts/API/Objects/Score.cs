using System;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	/// <summary>
	/// Score object.
	/// </summary>
	public sealed class Score : Base
	{
		#region Fields & Properties
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The numerical value of the score (i.e. 123).</value>
		public int Value { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The textual value of the score (i.e. "123 Jumps").</value>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the extra.
		/// </summary>
		/// <value>The extra logging information.</value>
		public string Extra { get; set; }

		/// <summary>
		/// Gets or sets the time.
		/// </summary>
		/// <value>The time.</value>
		public string Time { get; set; }

		/// <summary>
		/// Gets or sets the user ID.
		/// </summary>
		/// <value>The user ID.</value>
		public int UserID { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the name of the guest.
		/// </summary>
		/// <value>The name of the guest.</value>
		public string GuestName { get; set; }

		/// <summary>
		/// Gets the name of the player (username or guest name as appropriate).
		/// </summary>
		/// <value>The name of the player.</value>
		public string PlayerName { get { return !string.IsNullOrEmpty(UserName) ? UserName : GuestName; } }
		#endregion Fields & Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Score"/> class.
		/// </summary>
		/// <param name="value">The <see cref="Score"/> value.</param>
		/// <param name="text">The <see cref="Score"/> text.</param>
		/// <param name="guestName">The <see cref="Score"/> guest name.</param>
		/// <param name="extra">The <see cref="Score"/> extra.</param>
		public Score(int value, string text, string guestName = "", string extra = "")
		{
			Value = value;
			Text = text;
			GuestName = guestName;
			Extra = extra;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Score"/> class.
		/// </summary>
		/// <param name="data">API JSON data.</param>
		public Score(JSONClass data)
		{
			PopulateFromJSON(data);
		}
		#endregion Constructors
		
		#region Update Attributes
		/// <summary>
		/// Map JSON data to the object's attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		protected override void PopulateFromJSON(JSONClass data)
		{
			Value = data["sort"].AsInt;
			Text = data["score"].Value;
			Extra = data["extra_data"].Value;
			Time = data["stored"].Value;

			UserID = data["user_id"].AsInt;
			UserName = data["user"].Value;
			GuestName = data["guest"].Value;
		}
		#endregion Update Attributes

		#region Interface
		/// <summary>
		/// Add the <see cref="Score"/>.
		/// </summary>
		/// <param name="table">The ID of the HighScore <see cref="Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		/// <remarks>
		/// <para>
		/// Shortcut for <c>GameJolt.API.Scores.Add(this);</c>
		/// </para>
		/// </remarks>
		public void Add(int table = 0, Action<bool> callback = null)
		{
			Scores.Add(this, table, callback);
		}
		#endregion Interface

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="Score"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents the current <see cref="Score"/>.</returns>
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Score: {0} - {1}", PlayerName, Value);
		}
	}
}