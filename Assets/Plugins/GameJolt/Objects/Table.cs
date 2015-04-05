using SimpleJSON;

namespace GJAPI.Objects
{
	public class Table : Base
	{
		#region Fields & Properties
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Primary { get; private set; }
		#endregion Fields & Properties

		#region Constructors
		public Table(int id, string name, string description = "")
		{
			this.Id = id;
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
			this.Id = data["id"].AsInt;
			this.Name = data["name"].Value;
			this.Description = data["description"].Value;
			this.Primary = data["primary"].AsBool;
		}
		#endregion Update Attributes

		public override string ToString()
		{
			return string.Format("GJAPI.Objects.Table: {0} - {1}", Name, Id);
		}
	}
}