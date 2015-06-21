using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	public class Table : Base
	{
		#region Fields & Properties
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Primary { get; private set; }
		#endregion Fields & Properties

		#region Constructors
		public Table(int id, string name, string description = "")
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Primary = false;
		}

		public Table(JSONClass data)
		{
			this.PopulateFromJSON(data);
		}
		#endregion Constructors

		#region Update Attributes
		protected override void PopulateFromJSON(JSONClass data)
		{
			this.ID = data["id"].AsInt;
			this.Name = data["name"].Value;
			this.Description = data["description"].Value;
			this.Primary = data["primary"].Value == "1";
		}
		#endregion Update Attributes

		public override string ToString()
		{
			return string.Format("GameJolt.API.Objects.Table: {0} - {1}", Name, ID);
		}
	}
}