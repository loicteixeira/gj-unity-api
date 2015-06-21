using GameJolt.UI.Controllers;
using UnityEngine;
using System;

namespace GameJolt.UI
{
	[RequireComponent(typeof(Animator))]
	public class Manager : GameJolt.API.Core.MonoSingleton<Manager>
	{
		#region Init
		SignInWindow signinWindow;
		TrophiesWindow trophiesWindow;
		LeaderboardsWindow leaderboardsWindow;
		Behaviours.NotificationCentre notificationCentre;

		override protected void Init()
		{
			var animator = GetComponent<Animator>();
			notificationCentre = animator.GetBehaviour<Behaviours.NotificationCentre>();

			// GetComponentInChildren does not look in inactive childrens.
			// GetComponentsInChildren does look in inactive children but would alocate memory.
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

				if (leaderboardsWindow == null)
				{
					leaderboardsWindow = children.GetComponent<LeaderboardsWindow>();
					if (leaderboardsWindow != null)
					{
						leaderboardsWindow.Init(animator);
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

		#region Leaderboards
		public void ShowLeaderboards()
		{
			ShowLeaderboards(null);
		}

		public void ShowLeaderboards(Action<bool> callback)
		{
			leaderboardsWindow.Show(callback);
		}
		#endregion Leaderboards

		#region Notifications
		public void QueueNotification(string text)
		{
			var notification = new Objects.Notification(text);
			QueueNotification(notification);
		}

		public void QueueNotification(string text, Sprite image)
		{
			var notification = new Objects.Notification(text, image);
			QueueNotification(notification);
		}

		public void QueueNotification(Objects.Notification notification)
		{
			notificationCentre.QueueNotification(notification);
		}
		#endregion Notidications
	}
}
