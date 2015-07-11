using System;
using System.Collections.Generic;
using System.Text;

namespace GameJolt.API.Core
{
	public static class Request
	{
		public static void Get(
			string method,
		    Dictionary<string,string> parameters,
			Action<Core.Response> callback,
			bool requireVerified=true,
			Core.ResponseFormat format = Core.ResponseFormat.Json)
		{
			var error = Prepare(ref parameters, requireVerified, format);
			if (error != null)
			{
				callback(new Core.Response(error));
			}
			else
			{
				var url = GetRequestURL(method, parameters);
				Manager.Instance.StartCoroutine(Manager.Instance.GetRequest(url, format, callback));
			}
		}

		public static void Post(
			string method,
			Dictionary<string,string> parameters,
			Dictionary<string,string> payload,
			Action<Core.Response> callback,
			bool requireVerified=true,
			Core.ResponseFormat format = Core.ResponseFormat.Json)
		{
			var error = Prepare(ref parameters, requireVerified, format);
			if (error != null)
			{
				callback(new Core.Response(error));
			}
			else
			{
				var url = GetRequestURL(method, parameters);
				Manager.Instance.StartCoroutine(Manager.Instance.PostRequest(url, payload, format, callback));
			}
		}

		static string Prepare(ref Dictionary<string, string> parameters, bool requireVerified, Core.ResponseFormat format)
		{
			if (parameters == null)
			{
				parameters = new Dictionary<string, string>();
			}
			
			if (requireVerified)
			{
				if (Manager.Instance.CurrentUser == null || !Manager.Instance.CurrentUser.IsAuthenticated)
				{
					return "Missing Authenticated User.";
				}
				parameters.Add("username", Manager.Instance.CurrentUser.Name.ToLower());
				parameters.Add("user_token", Manager.Instance.CurrentUser.Token.ToLower());
			}
			
			parameters.Add("format", format.ToString().ToLower());

			return null;
		}

		static string GetRequestURL(string method, Dictionary<string,string> parameters)
		{
			StringBuilder url = new StringBuilder ();
			url.Append(Constants.API_BASE_URL);
			url.Append(method);
			url.Append("?game_id=");
			url.Append(Manager.Instance.GameID);
			
			foreach (KeyValuePair<string,string> parameter in parameters)
			{
				url.Append("&");
				url.Append(parameter.Key);
				url.Append("=");
				url.Append(parameter.Value.Replace(" ", "%20"));
			}
			
			string signature = GetSignature(url.ToString());
			url.Append("&signature=");
			url.Append(signature);
			
			return url.ToString();
		}

		static string GetSignature(string input)
		{
			return MD5(input + Manager.Instance.PrivateKey);
		}

		static string MD5(string input)
		{
			var bytes = Encoding.UTF8.GetBytes(input);

#if UNITY_WINRT
			var hashBytes = UnityEngine.Windows.Crypto.ComputeMD5Hash(bytes);
#else
			var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			var hashBytes = md5.ComputeHash(bytes);
#endif

			string hashString = "";
			for (int i=0; i < hashBytes.Length; i++)
			{
				hashString += hashBytes[i].ToString("x2").ToLower();
			}

			return hashString.PadLeft(32, '0');
		}
	}
}