using UnityEngine;
using System;

namespace GameJolt.UI.Controllers
{
	public abstract class BaseWindow : MonoBehaviour
	{
		protected Animator animator;

		public void Init(Animator animator)
		{
			this.animator = animator;
		}

		public abstract void Show(Action<bool> callback);

		public abstract void Dismiss(bool success);

		protected void Populate(RectTransform container, GameObject prefab, int count)
		{
			if (container.childCount < count)
			{
				int nbToCreate = count - container.childCount;
				for (int i = 0; i < nbToCreate; ++i)
				{
					var tr = Instantiate(prefab).transform;
					tr.SetParent(container);
					tr.SetAsLastSibling();
				}
			}
			else if (container.childCount > count)
			{
				int nbToDelete = container.childCount - count;
				for (int i = 0; i < nbToDelete; ++i)
				{
					DestroyImmediate(container.GetChild(0).gameObject);
				}
			}
		}
	}
}