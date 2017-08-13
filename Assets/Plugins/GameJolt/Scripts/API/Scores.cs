using UnityEngine;
using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	/// <summary>
	/// Scores API methods
	/// </summary>
	public static class Scores
	{
		/// <summary>
		/// The cached high score tables.
		/// </summary>
		static Objects.Table[] cachedTables = null;

		#region Add
		/// <summary>
		/// Add a <see cref="GameJolt.API.Objects.Score"/>.
		/// </summary>
		/// <param name="score">The <see cref="GameJolt.API.Objects.Score"/> to add.</param>
		/// <param name="table">The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Add(Objects.Score score, int table = 0, Action<bool> callback = null)
		{
			Add(score.Value, score.Text, score.GuestName, table, score.Extra, callback);
		}

		/// <summary>
		/// Add a <see cref="GameJolt.API.Objects.Score"/>.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="text">The textual value of the score (i.e. "123 Jumps").</param>
		/// <param name="table">The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="extra">Extra logging information. Defaults to an empty string.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Add(int value, string text, int table = 0, string extra = "", Action<bool> callback = null)
		{
			Add(value, text, "", table, extra, callback);
		}

		/// <summary>
		/// Add a score <see cref="GameJolt.API.Objects.Score"/>.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="text">The textual value of the score (i.e. "123 Jumps").</param>
		/// <param name="guestName">The guest name if guest score. Leave blank otherwise.</param>
		/// <param name="table">The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="extra">Extra logging information. Defaults to an empty string.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success..</param>
		public static void Add(int value, string text, string guestName, int table = 0, string extra = "", Action<bool> callback = null)
		{
			var guestScore = guestName != null && guestName != string.Empty;

			var parameters = new Dictionary<string, string>();
			parameters.Add("sort", value.ToString());
			parameters.Add("score", text);
			if (guestScore)
			{
				parameters.Add("guest", guestName);
			}
			if (table != 0)
			{
				parameters.Add("table_id", table.ToString());
			}
			if (extra != null && extra != string.Empty)
			{
				parameters.Add("extra_data", extra);
			}

			Core.Request.Get(Constants.API_SCORES_ADD, parameters, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, !guestScore);
		}
		#endregion Add

		#region Get
		/// <summary>
		/// Get the specified callback, table, limit and currentUserOnly.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of <see cref="GameJolt.API.Objects.Score"/>s</param>
		/// <param name="table">The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="limit">The maximum number of <see cref="GameJolt.API.Objects.Score"/>s to return. Defaults to 10.</param>
		/// <param name="currentUserOnly">If set to <c>true</c> only return scores of the current user; otherwise for all the users.</param>
		public static void Get(Action<Objects.Score[]> callback, int table = 0, int limit = 10, bool currentUserOnly = false)
		{
			var parameters = new Dictionary<string, string>();
			if (table != 0)
			{
				parameters.Add("table_id", table.ToString());
			}
			parameters.Add("limit", Math.Max(1, limit).ToString());

			Core.Request.Get(Constants.API_SCORES_FETCH, parameters, (Core.Response response) => {
				Objects.Score[] scores;
				if (response.success)
				{
					int count = response.json["scores"].AsArray.Count;
					scores = new Objects.Score[count];

					for (int i = 0; i < count; ++i)
					{
						scores[i] = new Objects.Score(response.json["scores"][i].AsObject);
					}
				}
				else
				{
					scores = null;
				}

				if (callback != null)
				{
					callback(scores);
				}
			}, currentUserOnly);
		}
		#endregion Get

		#region GetRank
		/// <summary>
		/// Get the score's rank.
		/// </summary>
		/// <param name="value">The numerical value of the score (i.e. 123).</param>
		/// <param name="table">The ID of the HighScore <see cref="GameJolt.API.Objects.Table"/>. Defaults to 0 (i.e. Primary Table).</param>
		/// <param name="callback">A callback function accepting a single parameter, an int.</param>
		public static void GetRank(int value, int table = 0, Action<int> callback = null)
		{
			var parameters = new Dictionary<string, string> ();
			if (table != 0)
			{
				parameters.Add("table_id", table.ToString());
			}
			parameters.Add("sort", value.ToString());

			Core.Request.Get(Constants.API_SCORES_RANK, parameters, (Core.Response response) => {
				if (callback != null)
				{
					int rank;
					if (response.success)
					{
						rank = response.json["rank"].AsInt;
					}
					else
					{
						rank = 0;
					}
					callback(rank);
				}
			}, false);
		}
		#endregion GetRank

		#region GetTables
		/// <summary>
		/// Get the high score tables.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, an array of HighScore <see cref="GameJolt.API.Objects.Table"/>s.</param>
		public static void GetTables(Action<Objects.Table[]> callback)
		{
			if (cachedTables != null)
			{
				if (callback != null)
				{
					callback(cachedTables);
				}
			}
			else
			{
				Core.Request.Get(Constants.API_SCORES_TABLES_FETCH, null, (Core.Response response) => {
					Objects.Table[] tables;
					if(response.success)
					{
						int count = response.json["tables"].AsArray.Count;
						tables = new Objects.Table[count];
						
						for (int i = 0; i < count; ++i)
						{
							tables[i] = new Objects.Table(response.json["tables"][i].AsObject);
						}
					}
					else
					{
						tables = null;
					}

					if (Manager.Instance.UseCaching)
					{
						cachedTables = tables;
					}
					
					if (callback != null)
					{
						callback(tables);
					}
				}, false);
			}
		}
		#endregion GetTables
	}
}
