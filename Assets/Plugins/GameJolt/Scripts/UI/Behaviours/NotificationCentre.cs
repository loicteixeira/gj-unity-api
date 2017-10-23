using UnityEngine;
using System.Collections.Generic;

namespace GameJolt.UI.Behaviours
{
	public class NotificationCentre : StateMachineBehaviour
	{
		public string notificationPanelPath;

		Controllers.NotificationItem notificationItem;
		Queue<Objects.Notification> notificationsQueue;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (notificationItem == null)
			{
				var panelTransform = animator.transform.Find(notificationPanelPath);
				if (panelTransform != null)
				{
					notificationItem = panelTransform.GetComponent<Controllers.NotificationItem>();
				}
			}

			if (notificationsQueue == null)
			{
				notificationsQueue = new Queue<Objects.Notification>();
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
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
