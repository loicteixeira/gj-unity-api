using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadTest : MonoBehaviour
{
	public void SignInButtonClicked()
	{
		GameJolt.UI.Manager.Instance.ShowSignIn((bool success) => {
			if (success)
			{
				GameJolt.UI.Manager.Instance.QueueNotification("Welcome");
			}
			else
			{
				GameJolt.UI.Manager.Instance.QueueNotification("Closed the window :(");
			}
		});
	}

	public void SignOutButtonClicked() {
		if(GameJolt.API.Manager.Instance.CurrentUser == null) {
			GameJolt.UI.Manager.Instance.QueueNotification("You're not signed in");
		} else {
			GameJolt.API.Manager.Instance.CurrentUser.SignOut();
			GameJolt.UI.Manager.Instance.QueueNotification("Signed out :(");
		}
	}

	public void IsSignedInButtonClicked() {
		if (GameJolt.API.Manager.Instance.CurrentUser != null) {
			GameJolt.UI.Manager.Instance.QueueNotification(
				"Signed in as " + GameJolt.API.Manager.Instance.CurrentUser.Name);
		}
		else {
			GameJolt.UI.Manager.Instance.QueueNotification("Not Signed In :(");
		}
	}

	public void LoadSceneButtonClicked(string sceneName) {
		Debug.Log("Loading Scene " + sceneName);
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		Application.LoadLevel(sceneName);
#else
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#endif
	}
}
