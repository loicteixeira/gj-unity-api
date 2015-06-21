using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	public enum SessionStatus { Active, Idle }

	public static class Sessions
	{
		public static void Open(Action<bool> callback = null)
		{
			Core.Request.Get(Constants.API_SESSIONS_OPEN, null, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, true, Core.ResponseFormat.Json);
		}

		public static void Ping(SessionStatus status = SessionStatus.Active, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("status", status.ToString().ToLower());

			Core.Request.Get(Constants.API_SESSIONS_PING, null, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, true, Core.ResponseFormat.Json);
		}

		public static void Close(Action<bool> callback = null)
		{
			Core.Request.Get(Constants.API_SESSIONS_CLOSE, null, (Core.Response response) => {
				if (callback != null)
				{
					callback(response.success);
				}
			}, true, Core.ResponseFormat.Json);
		}
	}
}
