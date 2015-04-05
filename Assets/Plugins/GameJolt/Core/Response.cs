using System.Collections.Generic;
using System.Linq;
using SimpleJSON;

namespace GJAPI.Core
{
	public enum ResponseFormat { KeyPair, Json, Dump }

	public class Response
	{			
		public readonly ResponseFormat format;
		public readonly bool success;
		public readonly string raw;
		public readonly string dump;
		public readonly JSONNode json;
		
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
				break;
				
			case ResponseFormat.Json:
				this.json = JSON.Parse(response)["response"];
				this.success = this.json["success"].AsBool;

				break;
			
			default:
				this.success = response.StartsWith("success:\"true\"");
				this.raw = response;

				break;
			}
		}
	}
}
