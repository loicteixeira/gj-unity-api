using System;
using System.Collections.Generic;

namespace GameJolt.API
{
	/// <summary>
	/// Session statuses.
	/// </summary>
	public enum SessionStatus { Active, Idle }

	/// <summary>
	/// Sessions API methods
	/// </summary>
	public static class Sessions
	{
		/// <summary>
		/// Open a session (on the GameJolt).
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Open(Action<bool> callback = null)
		{
			Core.Request.Get(Constants.API_SESSIONS_OPEN, null, response => {
				if (callback != null)
				{
					callback(response.success);
				}
			});
		}

		/// <summary>
		/// Ping (i.e. keep alive) a session (on the GameJolt).
		/// </summary>
		/// <param name="status">The <see cref="SessionStatus"/> to set the session to.</param>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public static void Ping(SessionStatus status = SessionStatus.Active, Action<bool> callback = null)
		{
			var parameters = new Dictionary<string, string> {{"status", status.ToString().ToLower()}};

			Core.Request.Get(Constants.API_SESSIONS_PING, parameters, response => {
				if (callback != null)
				{
					callback(response.success);
				}
			});
		}

		/// <summary>
		/// Close a session (on the GameJolt).
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success..</param>
		public static void Close(Action<bool> callback = null)
		{
			Core.Request.Get(Constants.API_SESSIONS_CLOSE, null, response => {
				if (callback != null)
				{
					callback(response.success);
				}
			});
		}
	}
}
