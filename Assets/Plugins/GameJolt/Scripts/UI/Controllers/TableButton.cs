using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.UI.Controllers
{
	public class TableButton : MonoBehaviour
	{
		public Text title;
		public Image backgroundImage;
		public Color defaultBackgroundColour = Color.grey;
		public Color activeBackgroundColour = Color.green;

		Button button;
		int tabIndex;
		LeaderboardsWindow windowController;
		bool active;

		public void Awake()
		{
			this.button = GetComponent<Button>();
		}

		public void Init(GameJolt.API.Objects.Table table, int index, LeaderboardsWindow controller, bool active = false)
		{
			title.text = table.Name;
			tabIndex = index;
			windowController = controller;
			SetActive(active);


			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(Clicked);
		}

		public void SetActive(bool active)
		{
			this.active = active;
			backgroundImage.color = active ? activeBackgroundColour : defaultBackgroundColour;
		}

		public void Clicked()
		{
			if (!active)
			{
				SetActive(!active);
				windowController.ShowTab(tabIndex);
			}
		}
	}
}