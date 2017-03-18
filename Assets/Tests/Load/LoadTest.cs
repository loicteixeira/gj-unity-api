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
				Debug.Log("Logged In");
			}
			else
			{
				Debug.Log("Dismissed");
			}
		});
	}

	public void IsSignedInButtonClicked() {
		bool isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
		if (isSignedIn) {
			Debug.Log("Signed In");
		}
		else {
			Debug.Log("Not Signed In");
		}
	}

	public void LoadSceneButtonClicked(string sceneName) {
		Debug.Log("Loading Scene " + sceneName);
		Application.LoadLevel(sceneName);
	}
}
