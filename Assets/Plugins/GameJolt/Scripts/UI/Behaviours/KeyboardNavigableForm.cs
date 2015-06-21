using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameJolt.UI.Behaviours
{
	public class KeyboardNavigableForm : StateMachineBehaviour
	{
		public string firstFieldPath;
		public string submitButtonPath;

		protected InputField firstField;
		protected Button submitButton;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (firstField == null)
			{
				var firstFieldTransform = animator.transform.Find(firstFieldPath);
				if (firstFieldTransform != null)
				{
					firstField = firstFieldTransform.GetComponent<InputField>();
				}
			}

			if (submitButton == null)
			{
				var submitButtonTransform = animator.transform.Find(submitButtonPath);
				if (submitButtonTransform != null)
				{
					submitButton = submitButtonTransform.GetComponent<Button>();
				}
			}

			if (firstField != null)
			{
				firstField.Select();
			}
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					NavigateUp();
				}
				else
				{
					NavigateDown();
				}
			}

			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				Submmit();
			}
		}

		virtual protected void NavigateUp()
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				return;
			}

			var next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			if (next != null)
			{
				next.Select();
			}
		}

		virtual protected void NavigateDown()
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				return;
			}

			var next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			if (next != null)
			{
				next.Select();
			}
		}
		
		virtual protected void Submmit()
		{
			if (submitButton != null)
			{
				var pointer = new PointerEventData(EventSystem.current);
				ExecuteEvents.Execute(submitButton.gameObject, pointer, ExecuteEvents.submitHandler);
			}
		}
	}
}
