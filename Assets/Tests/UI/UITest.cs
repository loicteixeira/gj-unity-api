using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour
{
	public void SignInButtonClicked()
	{
		GJAPI.UI.Manager.Instance.ShowSignIn((bool success) => {
			if (success)
			{
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
			GJAPI.Manager.Instance.CurrentUser.SignOut();
		}
	}
}
