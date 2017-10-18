using UnityEngine;

namespace GameJolt.UI.Behaviours
{
	public class DismissibleWindow : StateMachineBehaviour
	{
		public bool returnValue;

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				var baseWindow = animator.GetComponentInChildren<Controllers.BaseWindow>();
				// Because a window might be inactive (e.g. during transitions), it might be null.
				// It would be possible to manually loop through children and do GetComponent instead,
				// but dimissing an inactive/transisionning window might have some side effects,
				// so it's better off not dismissing it (it will be a bit weird to the user though).
				if (baseWindow != null) {
					baseWindow.Dismiss(returnValue);
				}
			}
		}
	}
}
