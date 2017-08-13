using GameJolt.UI.Controllers;
using UnityEngine;
using System;

namespace GameJolt.UI
{
	[RequireComponent(typeof(Animator))]
	/// <summary>
	/// The UI API Manager.
	/// </summary>
	public class Manager : GameJolt.API.Core.MonoSingleton<Manager>
	{
		#region Init
		SignInWindow signinWindow;
		TrophiesWindow trophiesWindow;
		LeaderboardsWindow leaderboardsWindow;
		Behaviours.NotificationCentre notificationCentre;

		/// <summary>
		/// Init this instance.
		/// </summary>
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
		/// <summary>
		/// Shows the sign in window.
		/// </summary>
		public void ShowSignIn()
		{
			ShowSignIn(null, null);
		}

		/// <summary>
		/// Shows the sign in windows.
		/// </summary>
		/// <param name="signedInCallback">A callback function accepting a single parameter, a boolean indicating whether the user has been signed-in successfully.</param>
		/// <param name="userFetchedCallback">A callback function accepting a single parameter, a boolean indicating whether the user's information have been fetched successfully.</param>
		public void ShowSignIn(Action<bool> signedInCallback = null, Action<bool> userFetchedCallback = null)
		{
			signinWindow.Show(signedInCallback, userFetchedCallback);
		}
		#endregion SignIn

		#region Trophies
		/// <summary>
		/// Shows the trophies windows.
		/// </summary>
		public void ShowTrophies()
		{
			ShowTrophies(null);
		}

		/// <summary>
		/// Shows the trophies windows.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public void ShowTrophies(Action<bool> callback)
		{
			trophiesWindow.Show(callback);
		}
		#endregion Trophies

		#region Leaderboards
		/// <summary>
		/// Shows the leaderboards window.
		/// </summary>
		public void ShowLeaderboards()
		{
			ShowLeaderboards(null);
		}

		/// <summary>
		/// Shows the leaderboards window.
		/// </summary>
		/// <param name="callback">A callback function accepting a single parameter, a boolean indicating success.</param>
		public void ShowLeaderboards(Action<bool> callback)
		{
			leaderboardsWindow.Show(callback);
		}
		#endregion Leaderboards

		#region Notifications
		/// <summary>
		/// Queues a notification.
		/// </summary>
		/// <param name="text">The notification text.</param>
		public void QueueNotification(string text)
		{
			var notification = new Objects.Notification(text);
			QueueNotification(notification);
		}

		/// <summary>
		/// Queues a notification.
		/// </summary>
		/// <param name="text">The notification text.</param>
		/// <param name="image">The notification image.</param>
		public void QueueNotification(string text, Sprite image)
		{
			var notification = new Objects.Notification(text, image);
			QueueNotification(notification);
		}

		/// <summary>
		/// Queues a notification.
		/// </summary>
		/// <param name="notification">The <see cref="GameJolt.UI.Objects.Notification"/>.</param>
		public void QueueNotification(Objects.Notification notification)
		{
			notificationCentre.QueueNotification(notification);
		}
		#endregion Notidications
	}
}
