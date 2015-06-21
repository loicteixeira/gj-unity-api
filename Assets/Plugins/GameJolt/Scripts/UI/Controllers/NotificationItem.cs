using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers
{
	public class NotificationItem : MonoBehaviour
	{
		public Image image;
		public Text text;

		public void Init(Objects.Notification notification)
		{
			text.text = notification.Text;

			if (notification.Image != null)
			{
				image.sprite = notification.Image;
				image.enabled = true;
			}
			else
			{
				image.enabled = false;
			}
		}
	}
}