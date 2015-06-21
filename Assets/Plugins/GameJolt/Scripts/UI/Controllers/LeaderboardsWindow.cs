using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameJolt.UI.Controllers
{
	public class LeaderboardsWindow: BaseWindow
	{
		public RectTransform tabsContainer;
		public GameObject tableButton;

		public ScrollRect scoresScrollRect;
		public GameObject scoreItem;
		
		Action<bool> callback;

		int[] tableIDs;
		int currentTab;
		
		override public void Show(Action<bool> callback)
		{
			animator.SetTrigger("Leaderboards");
			animator.SetTrigger("ShowLoadingIndicator");

			GameJolt.API.Scores.GetTables((GameJolt.API.Objects.Table[] tables) => {
				if (tables != null)
				{
					// Create the right number of children.
					Populate(tabsContainer, tableButton, tables.Length);

					// Update children's text. 
					tableIDs = new int[tables.Length];
					for (int i = 0; i < tables.Length; ++i)
					{
						tabsContainer.GetChild(i).GetComponent<TableButton>().Init(tables[i], i,this, tables[i].Primary);

						// Keep IDs information and current tab for use when switching tabs.
						tableIDs[i] = tables[i].ID;
						if (tables[i].Primary)
						{
							currentTab = i;
						}
					}

					animator.SetTrigger("Unlock");

					SetScores();
				}
				else
				{
					// TODO: Show error notification
					animator.SetTrigger("HideLoadingIndicator");
					Dismiss(false);
				}
			});
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

		public void ShowTab(int index)
		{
			// There is no need to set the new tab button active, it has been done internally when the button has been clicked.
			tabsContainer.GetChild(currentTab).GetComponent<TableButton>().SetActive(false);
			currentTab = index;

			animator.SetTrigger("Lock");
			animator.SetTrigger("ShowLoadingIndicator");

			// Request new scores.
			SetScores(tableIDs[currentTab]);
		}

		void SetScores(int tableID = 0)
		{
			GameJolt.API.Scores.Get((GameJolt.API.Objects.Score[] scores) => {
				if (scores != null)
				{
					scoresScrollRect.verticalNormalizedPosition = 0;

					// Create the right number of children.
					Populate(scoresScrollRect.content, scoreItem, scores.Length);
					
					// Update children's text.
					for (int i = 0; i < scores.Length; ++i)
					{
						scoresScrollRect.content.GetChild(i).GetComponent<ScoreItem>().Init(scores[i]);
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
			}, tableID, 50);
		}
	}
}