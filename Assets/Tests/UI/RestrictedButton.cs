using UnityEngine;
using UnityEngine.UI;

public class RestrictedButton : MonoBehaviour
{
	void Start()
	{
		var button = GetComponent<Button>();

		if (GJAPI.Manager.Instance.DebugAutoConnect)
		{
			button.interactable = true;
		}
		else
		{
			button.interactable = false;
		}
	}
}
