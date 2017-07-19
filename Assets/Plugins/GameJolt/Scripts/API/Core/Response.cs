using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
		public readonly bool success = false;

		/// <summary>
		/// The response bytes.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="GameJolt.API.Core.ResponseFormat" is `Raw`./> 
		/// </para>
		/// </remarks>
		public readonly byte[] bytes = null;

		/// <summary>
		/// The response dump.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="GameJolt.API.Core.ResponseFormat" is `Dump`./> 
		/// </para>
		/// </remarks>
		public readonly string dump = null;

		/// <summary>
		/// The response JSON.
		/// </summary>
		/// <para>
		/// Only populated when the <see cref="GameJolt.API.Core.ResponseFormat" is `Json`./> 
		/// </para>
		/// </remarks>
		public readonly JSONNode json = null;

		/// <summary>
		/// The response texture.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="GameJolt.API.Core.ResponseFormat" is `Texture`./> 
		/// </para>
		/// </remarks>
		public readonly Texture2D texture = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Core.Response"/> class.
		/// </summary>
		/// <param name="errorMessage">Error message.</param>
		public Response(string errorMessage) {
			this.success = false;
			Debug.LogWarning(errorMessage);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameJolt.API.Core.Response"/> class.
		/// </summary>
		/// <param name="www">The API Fesponse.</param>
		/// <param name="format">The format of the response.</param>
		public Response(WWW www, ResponseFormat format = ResponseFormat.Json)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				this.success = false;
				Debug.LogWarning(www.error);
				return;
			}

			this.format = format;

			switch (format)
			{
			case ResponseFormat.Dump:
				this.success = www.text.StartsWith("SUCCESS");

				var returnIndex = www.text.IndexOf ('\n');
				if (returnIndex != -1)
				{
					this.dump = www.text.Substring(returnIndex + 1);
				}

				if (!this.success)
				{
					Debug.LogWarning(this.dump);
					this.dump = null;
				}

				break;
				
			case ResponseFormat.Json:
				this.json = JSON.Parse(www.text)["response"];
				this.success = this.json["success"].AsBool;

				if (!this.success)
				{
					Debug.LogWarning(this.json["message"]);
					this.json = null;
				}

				break;
			
			case ResponseFormat.Raw:
				this.success = true;
				this.bytes = www.bytes;

				break;

			case ResponseFormat.Texture:
				this.success = true;
				this.texture = www.texture;

				break;

			default:
				this.success = false;
				Debug.LogWarning("Unknown format. Cannot process response.");

				break;
			}
		}
	}
}
