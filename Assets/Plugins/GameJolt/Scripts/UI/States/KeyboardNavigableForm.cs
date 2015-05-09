using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GJAPI.UI.States
{
	public class KeyboardNavigableForm : StateMachineBehaviour
	{
		public string firstFieldName;
		public string submitButtonName;

		protected InputField firstField;
		protected Button submitButton;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			GameObject go;
			
			go = GameObject.Find(firstFieldName);
			if (go != null)
			{
				firstField = go.GetComponent<InputField>();
				if (firstField != null)
				{
					firstField.Select();
				}
			}
			
			go = GameObject.Find(submitButtonName);
			if (go != null)
			{
				submitButton = go.GetComponent<Button>();
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
