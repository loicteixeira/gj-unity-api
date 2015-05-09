using GJAPI.UI.Controllers;
using UnityEngine;
using System;

namespace GJAPI.UI
{
	[RequireComponent(typeof(Animator))]
	public class Manager : Core.MonoSingleton<Manager>
	{
		SignInWindow signinWindow;

		override protected void Init()
		{
			var animator = GetComponent<Animator>();

			// GetComponentInChildren do look for inactive childrens.
			// GetComponentsInChildren would alocate memory.
			// Instead, looping over childrens for what we need.
			foreach (Transform children in transform)
			{
				if (signinWindow == null)
				{
					signinWindow = children.GetComponent<SignInWindow>();
					if (signinWindow != null)
					{
						signinWindow.Init(animator);
					}
				}
			}
		}

		public void ShowSignIn()
		{
			ShowSignIn(null);
		}

		public void ShowSignIn(Action<bool> callback)
		{
			signinWindow.Show(callback);
		}
	}
}
