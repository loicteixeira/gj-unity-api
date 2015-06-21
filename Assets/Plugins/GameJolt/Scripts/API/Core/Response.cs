using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Core
{
	public enum ResponseFormat { Dump, Json, Raw, Texture }

	public class Response
	{			
		public readonly ResponseFormat format;
		public readonly bool success = false;
		public readonly byte[] bytes = null;
		public readonly string dump = null;
		public readonly JSONNode json = null;
		public readonly Texture2D texture = null;

		public Response(string errorMessage) {
			this.success = false;
			Debug.LogWarning(errorMessage);
		}
		
		public Response(WWW www, ResponseFormat format = ResponseFormat.Json)
		{
			if (www.error != null)
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
