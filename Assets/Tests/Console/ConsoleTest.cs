using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ConsoleTest : MonoBehaviour
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
	public void SignIn()
	{
		Debug.Log("Sign In. Click to see source.");

		var user = new GameJolt.API.Objects.User(userNameField.text, userTokenField.text);
		user.SignIn((bool success) => {
			AddConsoleLine(string.Format("Sign In {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SignOut()
	{
		Debug.Log("Sign Out. Click to see source.");

		var isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
		if (isSignedIn)
		{
			GameJolt.API.Manager.Instance.CurrentUser.SignOut();
		}

		AddConsoleLine(string.Format("Sign Out {0}.", isSignedIn ? "Succeesful" : "Failed"));
	}

	public void SessionOpen()
	{
		Debug.Log("Session Open. Click to see source.");

		GameJolt.API.Sessions.Open((bool success) => {
			AddConsoleLine(string.Format("Session Open {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionPingActive()
	{
		Debug.Log("Session Ping Active. Click to see source.");

		GameJolt.API.Sessions.Ping(GameJolt.API.SessionStatus.Active, (bool success) => {
			AddConsoleLine(string.Format("Session Ping Active {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionPingIdle()
	{
		Debug.Log("Session Ping Idle. Click to see source.");

		GameJolt.API.Sessions.Ping(GameJolt.API.SessionStatus.Idle, (bool success) => {
			AddConsoleLine(string.Format("Session Ping Idle {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SessionClose()
	{
		Debug.Log("Session Close. Click to see source.");

		GameJolt.API.Sessions.Close((bool success) => {
			AddConsoleLine(string.Format("Session Close {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void GetTables()
	{
		Debug.Log("Get Tables. Click to see source.");

		GameJolt.API.Scores.GetTables((GameJolt.API.Objects.Table[] tables) => {
			if (tables != null)
			{
				foreach (var table in tables.Reverse<GameJolt.API.Objects.Table>())
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

			GameJolt.API.Scores.Add(scoreValue, scoreTextField.text, guestNameField.text, tableID, "", (bool success) => {
				AddConsoleLine(string.Format("Score Add (for Guest) {0}.", success ? "Succeesful" : "Failed"));
			});
		}
		else
		{
			Debug.Log("Add Score (for Guest). Click to see source.");

			var scoreValue = scoreValueField.text != string.Empty ? int.Parse(scoreValueField.text) : 0;
			var tableID = tableField.text != string.Empty ? int.Parse(tableField.text) : 0;

			GameJolt.API.Scores.Add(scoreValue, scoreTextField.text, tableID, "", (bool success) => {
				AddConsoleLine(string.Format("Score Add {0}.", success ? "Succeesful" : "Failed"));
			});
		}
	}

	public void GetScores()
	{
		Debug.Log("Get Scores. Click to see source.");

		var tableID = tableField.text != string.Empty ? int.Parse(tableField.text) : 0;
		var limit = limitField.text != string.Empty ? int.Parse(limitField.text) : 10;
		GameJolt.API.Scores.Get((GameJolt.API.Objects.Score[] scores) => {
			if (scores != null)
			{
				foreach (var score in scores.Reverse<GameJolt.API.Objects.Score>())
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
		GameJolt.API.Trophies.Unlock(trophyID, (bool success) => {
			AddConsoleLine(string.Format("Unlock Trophy {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void GetTrophies()
	{
		if (trophyIDsField.text.IndexOf(',') == -1)
		{
			Debug.Log("Get Single Trophy. Click to see source.");

			var trophyID = trophyIDsField.text != string.Empty ? int.Parse(trophyIDsField.text) : 0;
			GameJolt.API.Trophies.Get(trophyID, (GameJolt.API.Objects.Trophy trophy) => {
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

			GameJolt.API.Trophies.Get(trophyIDs, (GameJolt.API.Objects.Trophy[] trophies) => {
				if (trophies != null)
				{
					foreach (var trophy in trophies.Reverse<GameJolt.API.Objects.Trophy>())
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

		GameJolt.API.Trophies.Get((GameJolt.API.Objects.Trophy[] trophies) => {
			if (trophies != null)
			{
				foreach (var trophy in trophies.Reverse<GameJolt.API.Objects.Trophy>())
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

		GameJolt.API.Trophies.Get(unlockedTrophiesOnlyToggle.isOn, (GameJolt.API.Objects.Trophy[] trophies) => {
			if (trophies != null)
			{
				foreach (var trophy in trophies.Reverse<GameJolt.API.Objects.Trophy>())
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

		GameJolt.API.DataStore.Get(keyField.text, globalToggle.isOn, (string value) => {
			if (value != null)
			{
				AddConsoleLine(string.Format("> {0}", value));
			}
		});
	}

	public void GetDataStoreKeys()
	{
		Debug.Log("Get DataStore Keys. Click to see source.");

		GameJolt.API.DataStore.GetKeys(globalToggle.isOn, (string[] keys) => {
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

		GameJolt.API.DataStore.Delete(keyField.text, globalToggle.isOn, (bool success) => {
			AddConsoleLine(string.Format("Remove DataStore Key {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void SetDataStoreKey()
	{
		Debug.Log("Set DataStore Key. Click to see source.");

		GameJolt.API.DataStore.Set(keyField.text, valueField.text, globalToggle.isOn, (bool success) => {
			AddConsoleLine(string.Format("Set DataStore Key {0}.", success ? "Succeesful" : "Failed"));
		});
	}

	public void UpdateDataStoreKey()
	{
		GameJolt.API.DataStoreOperation mode;
		try
		{
			mode = (GameJolt.API.DataStoreOperation) System.Enum.Parse(typeof(GameJolt.API.DataStoreOperation), modeField.text);
		}
		catch
		{
			Debug.LogWarning("Wrong Mode. Should be Add, Subtract, Multiply, Divide, Append or Prepend.");
			return;
		}

		Debug.Log("Update DataStore Key. Click to see source.");

		GameJolt.API.DataStore.Update(keyField.text, valueField.text, mode, globalToggle.isOn, (string value) => {
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
		var settings = Resources.Load(GameJolt.API.Constants.SETTINGS_ASSET_NAME) as GameJolt.API.Settings;
		if (settings != null)
		{
			userNameField.text = settings.user;
			userTokenField.text = settings.token;
		}
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
