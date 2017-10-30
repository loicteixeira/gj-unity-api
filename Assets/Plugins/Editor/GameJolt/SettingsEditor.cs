using System;
using GameJolt.API;
using UnityEditor;

namespace GameJolt.Editor {
	[CustomEditor(typeof(Settings))]
	public class SettingsEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			var settings = target as Settings;
			if(settings == null) return;
			if(string.IsNullOrEmpty(settings.encryptionKey))
				settings.encryptionKey = GetRandomPassword();
			base.OnInspectorGUI();
		}

		private static string GetRandomPassword() {
			const int pwLength = 16;
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+-*/=?!§$%&";
			var pw = new char[pwLength];
			var rnd = new Random();
			for(int i = 0; i < pwLength; i++)
				pw[i] = chars[rnd.Next(chars.Length)];
			return new string(pw);
		}
	}
}
