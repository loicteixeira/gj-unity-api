using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers
{
	public class TrophyItem : MonoBehaviour
	{
		public CanvasGroup group;
		public Image image;
		public Text title;
		public Text description;

		public void Init(GameJolt.API.Objects.Trophy trophy)
		{
			group.alpha = trophy.Unlocked ? 1f : .6f;
			title.text = trophy.Title;
			description.text = trophy.Description;

			if (trophy.Image != null)
			{
				image.sprite = trophy.Image;
			}
			else
			{
				trophy.DownloadImage((success) => {
					if (success)
					{
						image.sprite = trophy.Image;
					}
				});
			}
		}
	}
}