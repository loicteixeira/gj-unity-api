@page tutorial Getting Started

# Setup

## Create your game on Game Jolt

1. Select *Dashboard* from the top menu.
2. Select *Add Game* from the sidebar.
3. Follow the steps. Just make sure to select *Unity* under *Engine/Language/Creation Tool*.
4. Select *Game API* from the game page menu. From there, you can navigate between the 5 categories:
   - *Overview:* See basic stats about your game.
   - *Trophies:* Create and view trophies. Notice how each trophy has and *ID*, you'll need this later.
   - *Scores:* Create and view highscores tables (and manage scores). There should already be a default table. Notice how each table has an *ID*, you'll need this later.
   - *Data Storage:* Manage data storage.
   - *API Settings:* This is where you can find you *Game ID* and your *Private Key*, you'll need this later.

## Import the package in Unity

1. [Download](@ref downloads) the Unity Package. 
2. Right click in the *Project* tab.
3. Select *Import Package > Custom Package...* and locate the *Unity Package*.

## Configure the API

### Settings

1. Select *Edit > Project Settings > Game Jolt API*.
2. Fill the *Game ID* and *Private Key*.

There's quite a few options here, let's get through them:

- *Timeout:* The time in seconds before an API call should timeout and return failure.
- *Auto Ping:* Automatically create and ping sessions once a user has been authentified. Turn it off to handle it yourself.
- *Use Caching:* Cache High Score Tables and Trophies information for faster display.
- *Auto Connect:* AutoConnect in the Editor as if the game was hosted on GameJolt.
- *User:* The username to use for AutoConnect.
- *Token:* The token to use for AutoConnect.

**Tip:** Leave your mouse above the option in the inspector to see its description.


### Prefab

1. Open your first scene (the first that will load).
2. Select *GameObject > Game Jolt API Manager* to add the prefab to the scene.

**Tip:** You can import the prefab in any scene for testing (so you don't have to load your game from the start everytime) but remember to remove it and only leave it in the very first scene.

# Sign in

## Web Player and WebGL builds
For *Web Player* or *WebGL* builds hosted on Game Jolt the player will be automatically signed in. This is how you check if a player is currently signed in or not (Guest).

```
bool isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
```

## Standalone Builds

### Default UI
For standalone builds, you have to sign the player yourself in. The easiest way is to display the default sign in form.

```
GameJolt.UI.Manager.UI.ShowSignIn();
```

You can also pass a callback to get notified whether the player has signed in or not.

```
GameJolt.UI.Manager.UI.ShowSignIn((bool success) => {
	if (success)
	{
		Debug.Log("The user signed in!");
	}
	else
	{
		Debug.Log("The user failed to signed in or closed the window :(");
	}
});
```

**Tip:** You can even show this form without a single line of code! If you have a *Sign In with Game Jolt* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowSignIn ()* from the dropdown.

### Custom UI
If you don't like the default UI and use some custom UI that better fit your game that's fine too! Once you've got the user name and token sign him in manually.

```
var user = new GameJolt.API.Objects.User(userName, userToken);
user.SignIn((bool success) => {
	if (success)
	{
		Debug.Log("Success!");
	}
	else
	{
		Debug.Log("The user failed to signed in :(");
	}
});
```

# Sign Out

```
var isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
if (isSignedIn)
{
	GameJolt.API.Manager.Instance.CurrentUser.SignOut();
}
```

# Trophies
Trophies need a signed in user.

**Tip:** Trophies can benefit a lot from caching. Don't forget to enable *Use Caching* in the API settings.

## Unlock
Unlocking a trophy will automatically show a notification. Future version of the API will allow you to turn this off.

```
GameJolt.API.Trophies.Unlock(trophyID, (bool success) => {
	if (success)
	{
		Debug.Log("Success!");
	}
	else
	{
		Debug.Log("Something went wrong");
	}
});
```

## Show
### Default UI
It's as simple as a single line!

```
GameJolt.UI.Manager.UI.ShowTrophies();
```

**Tip:** You can even show this window without a single line of code! If you have a *Trophies Collection* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowTrophies ()* from the dropdown.

### Custom UI
You can query one or more trophy and display it/them yourself.

Get one specific trophy

```
var trophyID = 123;
GameJolt.API.Trophies.Get(trophyID, (GameJolt.API.Objects.Trophy trophy) => {
	if (trophy != null)
	{
		Debug.Log(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
	}
});
```

Get multiple trophies.

```
GameJolt.API.Trophies.Get(trophyIDs, (GameJolt.API.Objects.Trophy[] trophies) => {
	if (trophies != null)
	{
		foreach (var trophy in trophies.Reverse<GameJolt.API.Objects.Trophy>())
		{
			Debug.Log(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
		}
		Debug.Log(string.Format("Found {0} trophies.", trophies.Length));
	}
});
```

Or get all of them!

```
GameJolt.API.Trophies.Get((GameJolt.API.Objects.Trophy[] trophies) => {
	if (trophies != null)
	{
		foreach (var trophy in trophies.Reverse<GameJolt.API.Objects.Trophy>())
		{
			Debug.Log(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
		}
		Debug.Log(string.Format("Found {0} trophies.", trophies.Length));
	}
});
```

# Scores
## Add
You can either add a score for a signed in player.

```
int scoreValue = 1337; // The actual score.
string scoreText = "1337 kills"; // A string representing the score to be shown on the website.
int tableID = 94328; // Set it to 0 for main highscore table.
string extraData = ""; // This will not be shown on the website. You can store any information.

GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) => {
	Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
});
```

Or a guest.

```
string guestName = "Arthur Dent";

GameJolt.API.Scores.Add(scoreValue, scoreText, guestName, tableID, extraData, (bool success) => {
	Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
});
```

## Show
### Default UI
Show all the highscore tables with a single line of code.

```
GameJolt.UI.Manager.Instance.ShowLeaderboards();
```

**Tip:** You can even show this window without a single line of code! If you have a *Leaderboards* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowLeaderboards ()* from the dropdown.

### Custom UI
Coming soon. In the meantime, you can have a look at the documentation.

# Ping, Data Store & more
Coming soon. In the meantime, you can have a look at the documentation.