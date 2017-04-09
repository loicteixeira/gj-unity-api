using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	/// <summary>
	/// Table object.
	/// </summary>
	public class Table : Base
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public string Name { get; set; }

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
		/// Gets a value indicating whether this <see cref="GameJolt.API.Objects.Table"/> is the primary table.
		/// </summary>
		/// <value><c>true</c> if primary; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// <para>
		/// Settings this will only affect your game and won't be saved to GameJolt.
		/// </para>
		/// </remarks>
		public bool Primary { get; private set; }
		#endregion Fields & Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Objects.Table"/> class.
		/// </summary>
		/// <param name="id">The <see cref="GameJolt.API.Objects.Table"/> ID.</param>
		/// <param name="name">The <see cref="GameJolt.API.Objects.Table"/> name.</param>
		/// <param name="description">The <see cref="GameJolt.API.Objects.Table"/> description.</param>
		public Table(int id, string name, string description = "")
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Primary = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Objects.Table"/> class.
		/// </summary>
		/// <param name="data">API JSON data.</param>
		public Table(JSONClass data)
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
			this.Name = data["name"].Value;
			this.Description = data["description"].Value;
			this.Primary = data["primary"].Value == "1";
		}
		#endregion Update Attributes

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="GameJolt.API.Objects.Table"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="GameJolt.API.Objects.Table"/>.</returns>
		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Table: {0} - {1}", Name, ID);
		}
	}
}