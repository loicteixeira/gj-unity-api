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

		Action<bool> callback;

		override public void Show(Action<bool> callback)
		{
			errorMessage.enabled = false;
			animator.SetTrigger("SignIn");
			this.callback = callback;
		}

		override public void Dismiss(bool success)
		{
			animator.SetTrigger("Dismiss");
			if (callback != null)
			{
				callback(success);
				callback = null;
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
				user.SignIn((bool success) => {
					if (success)
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
				});
			}
		}
	}
}