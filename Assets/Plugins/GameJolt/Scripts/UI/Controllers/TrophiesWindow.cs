using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameJolt.UI.Controllers
{
	public class TrophiesWindow: BaseWindow
	{
		public RectTransform container;
		public GameObject trophyItem;

		Action<bool> callback;

		override public void Show(Action<bool> callback)
		{
			animator.SetTrigger("Trophies");
			animator.SetTrigger("ShowLoadingIndicator");
			GameJolt.API.Trophies.Get((GameJolt.API.Objects.Trophy[] trophies) => {
				if (trophies != null)
				{
					// Create children if there are none.
					if (container.childCount == 0)
					{
						Transform tr;
						for (int i = 0; i < trophies.Length; ++i)
						{
							tr = Instantiate(trophyItem).transform;
							tr.SetParent(container);
							tr.SetAsLastSibling();
						}
					}

					// Update children's text.
					for (int i = 0; i < trophies.Length; ++i)
					{
						container.GetChild(i).GetComponent<TrophyItem>().Init(trophies[i]);
					}

					animator.SetTrigger("HideLoadingIndicator");
					animator.SetTrigger("Unlock");
				}
				else
				{
					// TODO: Show error notification
					animator.SetTrigger("HideLoadingIndicator");
					Dismiss(false);
				}
			});

			this.callback = callback;
		}
		
		override public void Dismiss(bool success)
		{
			animator.SetTrigger("Dismiss");
			if (callback != null)
			{
				callback(success);
				callback = null;
			}
		}
	}
}