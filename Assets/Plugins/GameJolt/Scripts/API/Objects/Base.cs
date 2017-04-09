using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Objects
{
	/// <summary>
	/// Base class for all Objects.
	/// </summary>
	public abstract class Base
	{
		#region Update Attributes
		/// <summary>
		/// Bulks update the object attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		public void BulkUpdate(JSONClass data)
		{
			PopulateFromJSON(data);
		}

		/// <summary>
		/// Map JSON data to the object's attributes.
		/// </summary>
		/// <param name="data">JSON data from the API calls.</param>
		protected abstract void PopulateFromJSON(JSONClass data);
		#endregion Update Attributes
	}
}
