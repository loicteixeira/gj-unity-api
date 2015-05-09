using UnityEngine;
using UnityEngine.UI;
using System;

namespace GJAPI.UI.Controllers
{
	abstract public class BaseWindow : MonoBehaviour
	{
		protected Animator animator;

		public void Init(Animator animator)
		{
			this.animator = animator;
		}

		abstract public void Show(Action<bool> callback);

		abstract public void Dismiss(bool success);
	}
}