using UnityEngine;
using System.Collections.Generic;

namespace GameJolt.UI.Behaviours
{
	public class NotificationCentre : StateMachineBehaviour
	{
		public string notificationPanelPath;

		GameJolt.UI.Controllers.NotificationItem notificationItem;
		Queue<GameJolt.UI.Objects.Notification> notificationsQueue;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (notificationItem == null)
			{
				var panelTransform = animator.transform.Find(notificationPanelPath);
				if (panelTransform != null)
				{
					notificationItem = panelTransform.GetComponent<GameJolt.UI.Controllers.NotificationItem>();
				}
			}

			if (notificationsQueue == null)
			{
				notificationsQueue = new Queue<GameJolt.UI.Objects.Notification>();
			}
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (notificationsQueue.Count > 0)
			{
				var notification = notificationsQueue.Dequeue();
				notificationItem.Init(notification);
				animator.SetTrigger("Notification");
			}
		}

		public void QueueNotification(Objects.Notification notification)
		{
			notificationsQueue.Enqueue(notification);
		}
	}
}
