using UnityEngine;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Core
{
	/// <summary>
	/// API Response Formats.
	/// </summary>
	public enum ResponseFormat { Dump, Json, Raw, Texture }

	/// <summary>
	/// Response object to parse API responses.
	/// </summary>
	public class Response
	{
		/// <summary>
		/// The Response Format.
		/// </summary>
		public readonly ResponseFormat format;

		/// <summary>
		/// Whether the response is successful.
		/// </summary>
		public readonly bool success;

		/// <summary>
		/// The response bytes.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Raw`. 
		/// </para>
		/// </remarks>
		public readonly byte[] bytes;

		/// <summary>
		/// The response dump.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Dump`.
		/// </para>
		/// </remarks>
		public readonly string dump;

		/// <summary>
		/// The response JSON.
		/// </summary>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Json`.
		/// </para>
		/// </remarks>
		public readonly JSONNode json;

		/// <summary>
		/// The response texture.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Texture`. 
		/// </para>
		/// </remarks>
		public readonly Texture2D texture;

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="errorMessage">Error message.</param>
		public Response(string errorMessage) {
			success = false;
			Debug.LogWarning(errorMessage);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="www">The API Fesponse.</param>
		/// <param name="format">The format of the response.</param>
		public Response(WWW www, ResponseFormat format = ResponseFormat.Json)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				success = false;
				Debug.LogWarning(www.error);
				return;
			}

			this.format = format;

			switch (format)
			{
			case ResponseFormat.Dump:
				success = www.text.StartsWith("SUCCESS");
				var returnIndex = www.text.IndexOf ('\n');
				if (returnIndex != -1)
				{
					dump = www.text.Substring(returnIndex + 1);
				}

				if (!success)
				{
					Debug.LogWarning(dump);
					dump = null;
				}
				break;
				
			case ResponseFormat.Json:
				json = JSON.Parse(www.text)["response"];
				success = json["success"].AsBool;
				if (!success)
				{
					Debug.LogWarning(json["message"]);
					json = null;
				}
				break;
			
			case ResponseFormat.Raw:
				success = true;
				bytes = www.bytes;
				break;

			case ResponseFormat.Texture:
				success = true;
				texture = www.texture;
				break;

			default:
				success = false;
				Debug.LogWarning("Unknown format. Cannot process response.");
				break;
			}
		}
	}
}
