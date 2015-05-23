using UnityEngine;

namespace GJAPI.UI.States
{
	public class DismissibleWindow : StateMachineBehaviour
	{
		public bool returnValue = false;

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				animator.GetComponentInChildren<UI.Controllers.BaseWindow>().Dismiss(returnValue);
			}
		}
	}
}
