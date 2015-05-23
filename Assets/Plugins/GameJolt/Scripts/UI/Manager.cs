using GJAPI.UI.Controllers;
using UnityEngine;
using System;

namespace GJAPI.UI
{
	[RequireComponent(typeof(Animator))]
	public class Manager : Core.MonoSingleton<Manager>
	{
		#region Init
		SignInWindow signinWindow;
		TrophiesWindow trophiesWindow;

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

				if (trophiesWindow == null)
				{
					trophiesWindow = children.GetComponent<TrophiesWindow>();
					if (trophiesWindow != null)
					{
						trophiesWindow.Init(animator);
					}
				}
			}
		}
		#endregion Init

		#region SignIn
		public void ShowSignIn()
		{
			ShowSignIn(null);
		}

		public void ShowSignIn(Action<bool> callback)
		{
			signinWindow.Show(callback);
		}
		#endregion SignIn

		#region Trophies
		public void ShowTrophies()
		{
			ShowTrophies(null);
		}

		public void ShowTrophies(Action<bool> callback)
		{
			trophiesWindow.Show(callback);
		}
		#endregion Trophies
	}
}
