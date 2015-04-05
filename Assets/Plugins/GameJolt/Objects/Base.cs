using SimpleJSON;

namespace GJAPI.Objects
{
	public abstract class Base
	{
		#region Update Attributes
		public void BulkUpdate(JSONClass data)
		{
			PopulateFromJSON(data);
		}

		protected abstract void PopulateFromJSON(JSONClass data);
		#endregion Update Attributes
	}
}
