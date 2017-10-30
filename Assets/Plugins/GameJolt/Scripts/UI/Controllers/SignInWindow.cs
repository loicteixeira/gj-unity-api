using UnityEngine.UI;
using System;

namespace GameJolt.UI.Controllers
{
	public class SignInWindow: BaseWindow
	{
		public InputField usernameField;
		public InputField tokenField;
		public Text errorMessage;
		public Toggle rememberMeToggle;

		Action<bool> signedInCallback;
		Action<bool> userFetchedCallback;

		public override void Show(Action<bool> callback)
		{
			Show(callback, null);
		}

		public void Show(Action<bool> signedInCallback, Action<bool> userFetchedCallback) {
			errorMessage.enabled = false;
			animator.SetTrigger("SignIn");
			this.signedInCallback = signedInCallback;
			this.userFetchedCallback = userFetchedCallback;
			string username, token;
			rememberMeToggle.isOn = API.Manager.Instance.GetStoredUserCredentials(out username, out token);
			usernameField.text = username;
			tokenField.text = token;
		}

		public override void Dismiss(bool success)
		{
			animator.SetTrigger("Dismiss");
			if (signedInCallback != null)
			{
				signedInCallback(success);
				signedInCallback = null;
			}
		}

		public void Submit()
		{
			errorMessage.enabled = false;

			if (usernameField.text.Trim() == string.Empty || tokenField.text.Trim() == string.Empty)
			{
				errorMessage.text = "Empty username and/or token.";
				errorMessage.enabled = true;
			}
			else
			{
				animator.SetTrigger("Lock");
				animator.SetTrigger("ShowLoadingIndicator");

				var user = new API.Objects.User(usernameField.text.Trim(), tokenField.text.Trim());
				user.SignIn(signInSuccess => {
					if(signInSuccess) {
						Dismiss(true);
					} else {
						// Technically this could be because of another user being already signed in.
						errorMessage.text = "Wrong username and/or token.";
						errorMessage.enabled = true;
					}

					animator.SetTrigger("HideLoadingIndicator");
					animator.SetTrigger("Unlock");
				}, userFetchedSuccess => {
					if(userFetchedCallback != null) {
						// This will potentially be called after a user dismissed the window..
						userFetchedCallback(userFetchedSuccess);
						userFetchedCallback = null;
					}
				}, rememberMeToggle.isOn);
			}
		}
	}
}