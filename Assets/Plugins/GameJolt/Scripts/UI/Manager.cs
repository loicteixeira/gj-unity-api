using GJAPI.UI.Controllers;
using UnityEngine;
using System;

namespace GJAPI.UI
{
	[RequireComponent(typeof(Animator))]
	public class Manager : MonoBehaviour
	{
		#region Singleton
		protected static Manager instance;
		public static Manager Instance
		{
			get
			{
				if (instance == null) 
				{
					instance = FindObjectOfType<Manager>();
					
					if (instance == null)
					{
						Debug.LogError("An instance of " + typeof(Manager) + " is needed in the scene, but there is none.");
					}
				}
				
				return instance;
			}
		}
		#endregion Singleton

		SignInWindow signinWindow;

		void Awake()
		{
			if (Persist())
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
		}

		bool Persist()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(this.gameObject);
				return false;
			}
			
			DontDestroyOnLoad(this.gameObject);
			return true;
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
