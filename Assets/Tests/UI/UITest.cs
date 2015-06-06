using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITest : MonoBehaviour
{
	public Button showTrophiesButton;

	int notificationQueued = 0;

	public void SignInButtonClicked()
	{
		GJAPI.UI.Manager.Instance.ShowSignIn((bool success) => {
			if (success)
			{
				showTrophiesButton.interactable = true;
				Debug.Log("Logged In");
			}
			else
			{
				Debug.Log("Dismissed");
			}
		});
	}

	public void SignOutButtonClicked()
	{
		if (GJAPI.Manager.Instance.CurrentUser != null)
		{
			showTrophiesButton.interactable = false;
			GJAPI.Manager.Instance.CurrentUser.SignOut();
		}
	}

	public void QueueNotification()
	{
		GJAPI.UI.Manager.Instance.QueueNotification(
			string.Format("Notification <b>#{0}</b>", ++notificationQueued));
	}
}
