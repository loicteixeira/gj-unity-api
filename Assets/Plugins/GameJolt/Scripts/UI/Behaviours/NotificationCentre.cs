using UnityEngine;
using System.Collections.Generic;

namespace GJAPI.UI.Behaviours
{
	public class NotificationCentre : StateMachineBehaviour
	{
		public string notificationPanelPath;

		GJAPI.UI.Controllers.NotificationItem notificationItem;
		Queue<GJAPI.UI.Objects.Notification> notificationsQueue;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (notificationItem == null)
			{
				var panelTransform = animator.transform.Find(notificationPanelPath);
				if (panelTransform != null)
				{
					notificationItem = panelTransform.GetComponent<GJAPI.UI.Controllers.NotificationItem>();
				}
			}

			if (notificationsQueue == null)
			{
				notificationsQueue = new Queue<GJAPI.UI.Objects.Notification>();
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
