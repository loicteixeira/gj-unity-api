using UnityEngine;
using UnityEngine.UI;

public class RestrictedButton : MonoBehaviour
{
	void Start()
	{
		var button = GetComponent<Button>();

#if UNITY_EDITOR
		if (GameJolt.API.Manager.Instance.DebugAutoConnect)
		{
			button.interactable = true;
		}
		else
		{
			button.interactable = false;
		}
#else
		button.interactable = true;
#endif
	}
}
