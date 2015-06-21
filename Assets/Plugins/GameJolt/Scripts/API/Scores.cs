using UnityEngine;
using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	public static class Scores
	{
		static Objects.Table[] cachedTables = null;

		#region Add
		public static void Add(Objects.Score score, int table = 0, Action<bool> callback = null)
		{
			Add(score.Value, score.Text, score.GuestName, table, score.Extra, callback);
		}

		public static void Add(int value, string text, int table = 0, string extra = "", Action<bool> callback = null)
		{
			Add(value, text, "", table, extra, callback);
		}

		public static void Add(int value, string text, string guestName = "Guest", int table = 0, string extra = "", Action<bool> callback = null)
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

		#region GetTables
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
