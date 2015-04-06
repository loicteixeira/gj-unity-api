using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CSharpConsole : MonoBehaviour
{
	#region Inspector Fields
	// Console
	public RectTransform consoleTransform;
	public GameObject linePrefab;

	// Users
	public InputField userNameField;
	public InputField userTokenField;

	// Scores
	public InputField scoreValueField;
	public InputField scoreTextField;
	public Toggle guestScoreToggle;
	public InputField guestNameField;
	public InputField tableField;
	public InputField limitField;
	public Toggle userScoresToggle;

	// Trophies
	public InputField trophyIDField;
	public InputField trophyIDsField;
	public Toggle unlockedTrophiesOnlyToggle;

	// DataStore
	public InputField keyField;
	public InputField valueField;
	public InputField modeField;
	public Toggle globalToggle;
	#endregion Inspector Fields

	#region Click Actions
	public void Login()
	{
		Debug.Log("Login. Click to see source.");

		GJAPI.Users.Authenticate(userNameField.text, userTokenField.text, (bool success) => {
			AddConsoleLine(string.Format("Login {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionOpen()
	{
		Debug.Log("Session Open. Click to see source.");

		GJAPI.Sessions.Open((bool success) => {
			AddConsoleLine(string.Format("Session Open {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionPingActive()
	{
		Debug.Log("Session Ping Active. Click to see source.");

		GJAPI.Sessions.Ping(GJAPI.SessionStatus.Active, (bool success) => {
			AddConsoleLine(string.Format("Session Ping Active {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionPingIdle()
	{
		Debug.Log("Session Ping Idle. Click to see source.");

		GJAPI.Sessions.Ping(GJAPI.SessionStatus.Idle, (bool success) => {
			AddConsoleLine(string.Format("Session Ping Idle {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionClose()
	{
		Debug.Log("Session Close. Click to see source.");

		GJAPI.Sessions.Close((bool success) => {
			AddConsoleLine(string.Format("Session Close {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void GetTables()
	{
		Debug.Log("Get Tables. Click to see source.");

		GJAPI.Scores.GetTables((GJAPI.Objects.Table[] tables) => {
			if (tables != null)
			{
				foreach (var table in tables.Reverse<GJAPI.Objects.Table>())
				{
					AddConsoleLine(string.Format("> {0} - {1}", table.Name, table.ID));
				}
				AddConsoleLine(string.Format("Found {0} table(s).", tables.Length));
			}
		});
	}

	public void AddScore()
	{
		if (guestScoreToggle.isOn)
		{
			Debug.Log("Add Score (for Guest). Click to see source.");

			var scoreValue = scoreValueField.text != string.Empty ? int.Parse(scoreValueField.text) : 0;
			var tableID = tableField.text != string.Empty ? int.Parse(tableField.text) : 0;

			GJAPI.Scores.Add(scoreValue, scoreTextField.text, guestNameField.text, tableID, "", (bool success) => {
				AddConsoleLine(string.Format("Score Add (for Guest) {0}.", success ? "Succeesful" : "Failed"));
			});
		}
		else
		{
			Debug.Log("Add Score (for Guest). Click to see source.");

			var scoreValue = scoreValueField.text != string.Empty ? int.Parse(scoreValueField.text) : 0;
			var tableID = tableField.text != string.Empty ? int.Parse(tableField.text) : 0;

			GJAPI.Scores.Add(scoreValue, scoreTextField.text, tableID, "", (bool success) => {
				AddConsoleLine(string.Format("Score Add {0}.", success ? "Succeesful" : "Failed"));
			});
		}
	}

	public void GetScores()
	{
		Debug.Log("Get Scores. Click to see source.");

		var tableID = tableField.text != string.Empty ? int.Parse(tableField.text) : 0;
		var limit = limitField.text != string.Empty ? int.Parse(limitField.text) : 10;
		GJAPI.Scores.Get((GJAPI.Objects.Score[] scores) => {
			if (scores != null)
			{
				foreach (var score in scores.Reverse<GJAPI.Objects.Score>())
				{
					AddConsoleLine(string.Format("> {0} - {1}", score.PlayerName, score.Value));
				}
				AddConsoleLine(string.Format("Found {0} scores(s).", scores.Length));
			}
		}, tableID, limit, userScoresToggle.isOn);
	}

	public void UnlockTrophy()
	{
		Debug.Log("Unlock Trophy. Click to see source.");

		var trophyID = trophyIDField.text != string.Empty ? int.Parse(trophyIDField.text) : 0;
		GJAPI.Trophies.Unlock(trophyID, (bool success) => {
			AddConsoleLine(string.Format("Unlock Trophy {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void GetTrophies()
	{
		if (trophyIDsField.text.IndexOf(',') == -1)
		{
			Debug.Log("Get Single Trophy. Click to see source.");

			var trophyID = trophyIDsField.text != string.Empty ? int.Parse(trophyIDsField.text) : 0;
			GJAPI.Trophies.Get(trophyID, (GJAPI.Objects.Trophy trophy) => {
				if (trophy != null)
				{
					AddConsoleLine(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
				}
			});
		}
		else
		{
			Debug.Log("Get Multiple Trophies. Click to see source.");

			var idStrings = trophyIDsField.text.Split(',');
			var trophyIDs = new int[idStrings.Length];
			for (int i = 0; i < idStrings.Length; ++i)
			{
				trophyIDs[i] = idStrings[i] != string.Empty ? int.Parse(idStrings[i]) : 0;
			}

			GJAPI.Trophies.Get(trophyIDs, (GJAPI.Objects.Trophy[] trophies) => {
				if (trophies != null)
				{
					foreach (var trophy in trophies.Reverse<GJAPI.Objects.Trophy>())
					{
						AddConsoleLine(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
					}
					AddConsoleLine(string.Format("Found {0} trophies.", trophies.Length));
				}
			});
		}
	}

	public void GetAllTrophies()
	{
		Debug.Log("Get All Trophies. Click to see source.");

		GJAPI.Trophies.Get((GJAPI.Objects.Trophy[] trophies) => {
			if (trophies != null)
			{
				foreach (var trophy in trophies.Reverse<GJAPI.Objects.Trophy>())
				{
					AddConsoleLine(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
				}
				AddConsoleLine(string.Format("Found {0} trophies.", trophies.Length));
			}
		});
	}

	public void GetTrophiesByStatus()
	{
		Debug.Log("Get Trophies by Status (Unlocked or not). Click to see source.");

		GJAPI.Trophies.Get(unlockedTrophiesOnlyToggle.isOn, (GJAPI.Objects.Trophy[] trophies) => {
			if (trophies != null)
			{
				foreach (var trophy in trophies.Reverse<GJAPI.Objects.Trophy>())
				{
					AddConsoleLine(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
				}
				AddConsoleLine(string.Format("Found {0} trophies.", trophies.Length));
			}
		});
	}

	public void GetDataStoreKey()
	{
		Debug.Log("Get DataStore Key. Click to see source.");

		GJAPI.DataStore.Get(keyField.text, globalToggle.isOn, (string value) => {
			if (value != null)
			{
				AddConsoleLine(string.Format("> {0}", value));
			}
		});
	}

	public void GetDataStoreKeys()
	{
		Debug.Log("Get DataStore Keys. Click to see source.");

		GJAPI.DataStore.GetKeys(globalToggle.isOn, (string[] keys) => {
			if (keys != null)
			{
				foreach (var key in keys)
				{
					AddConsoleLine(string.Format("> {0}", key));
				}
				AddConsoleLine(string.Format("Found {0} keys.", keys.Length));
			}
			else
			{
				AddConsoleLine("No keys found.");
			}
		});
	}

	public void RemoveDataStoreKey()
	{
		Debug.Log("Remove DataStore Key. Click to see source.");

		GJAPI.DataStore.Remove(keyField.text, globalToggle.isOn, (bool success) => {
			AddConsoleLine(string.Format("Remove DataStore Key {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SetDataStoreKey()
	{
		Debug.Log("Set DataStore Key. Click to see source.");

		GJAPI.DataStore.Set(keyField.text, valueField.text, globalToggle.isOn, (bool success) => {
			AddConsoleLine(string.Format("Set DataStore Key {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void UpdateDataStoreKey()
	{
		GJAPI.DataStoreOperation mode;
		try
		{
			mode = (GJAPI.DataStoreOperation) System.Enum.Parse(typeof(GJAPI.DataStoreOperation), modeField.text);
		}
		catch
		{
			Debug.LogWarning("Wrong Mode. Should be Add, Subtract, Multiply, Divide, Append or Prepend.");
			return;
		}

		Debug.Log("Update DataStore Key. Click to see source.");

		GJAPI.DataStore.Update(keyField.text, valueField.text, mode, globalToggle.isOn, (string value) => {
			if (value != null)
			{
				AddConsoleLine(string.Format("> {0}", value));
			}
		});
	}
	#endregion Click Actions

	#region Internal
	void Start()
	{
		// Do not try this at home! Seriously, you shouldn't.
		var settings = Resources.Load(GJAPI.Constants.SETTINGS_ASSET_NAME) as GJAPI.Settings;
		userNameField.text = settings.user;
		userTokenField.text = settings.token;
	}

	void AddConsoleLine(string text)
	{
		var tr = Instantiate<GameObject>(linePrefab).transform;
		tr.GetComponent<Text>().text = text;
		tr.SetParent(consoleTransform);
		tr.SetAsFirstSibling();
	}
	#endregion Internal
}
