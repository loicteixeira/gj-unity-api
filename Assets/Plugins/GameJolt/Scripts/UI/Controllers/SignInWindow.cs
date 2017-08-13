using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameJolt.UI.Controllers
{
	public class SignInWindow: BaseWindow
	{
		public InputField usernameField;
		public InputField tokenField;
		public Text errorMessage;

		Action<bool> signedInCallback;
		Action<bool> userFetchedCallback;

		override public void Show(Action<bool> callback)
		{
			Show(callback, null);
		}

		public void Show(Action<bool> signedInCallback, Action<bool> userFetchedCallback)
		{
			errorMessage.enabled = false;
			animator.SetTrigger("SignIn");
			this.signedInCallback = signedInCallback;
			this.userFetchedCallback = userFetchedCallback;
		}

		override public void Dismiss(bool success)
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

				var user = new GameJolt.API.Objects.User(usernameField.text.Trim(), tokenField.text.Trim());
				user.SignIn((bool signInSuccess) => {
					if (signInSuccess)
					{
						Dismiss(true);
					}
					else
					{
						// Technically this could be because of another user being already signed in.
						errorMessage.text = "Wrong username and/or token.";
						errorMessage.enabled = true;
					}

					animator.SetTrigger("HideLoadingIndicator");
					animator.SetTrigger("Unlock");
				}, (bool userFetchedSuccess) => {
					if (userFetchedCallback != null) {
						// This will potentially be called after a user dismissed the window..
						userFetchedCallback(userFetchedSuccess);
						userFetchedCallback = null;
					}
				});
			}
		}
	}
}