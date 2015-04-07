using System.Collections.Generic;
using System.Linq;
using GJAPI.External.SimpleJSON;

namespace GJAPI.Core
{
	public enum ResponseFormat { KeyPair, Json, Dump }

	public class Response
	{			
		public readonly ResponseFormat format;
		public readonly bool success = false;
		public readonly string raw = null;
		public readonly string dump = null;
		public readonly JSONNode json = null;
		
		public Response(string response, ResponseFormat format = ResponseFormat.Json)
		{
			this.format = format;
			
			switch (format)
			{
			case ResponseFormat.Dump:
				this.success = response.StartsWith("SUCCESS");

				var returnIndex = response.IndexOf ('\n');
				if (returnIndex != -1)
				{
					this.dump = response.Substring(returnIndex + 1);
				}

				if (!this.success)
				{
					UnityEngine.Debug.LogWarning(this.dump);
					this.dump = null;
				}

				break;
				
			case ResponseFormat.Json:
				this.json = JSON.Parse(response)["response"];
				this.success = this.json["success"].AsBool;

				if (!this.success)
				{
					UnityEngine.Debug.LogWarning(this.json["message"]);
					this.json = null;
				}

				break;
			
			case ResponseFormat.KeyPair:
			default:
				this.success = response.StartsWith("success:\"true\"");

				if (this.success)
				{
					// Note: 
					this.raw = response;
				}
				else
				{
					var lines = response.Split('\n');
					foreach (var line in lines)
					{
						if (line != string.Empty)
						{
							var pair = line.Split(':');
							if (pair.Length >= 1 && pair[0] == "message")
							{
								UnityEngine.Debug.LogWarning(pair[1]);
								break;
							}
						}
					}
				}

				break;
			}
		}
	}
}
