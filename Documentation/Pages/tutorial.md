@page tutorial Getting Started

[1]: @ref documentation
[2]: @ref downloads

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

1. [Download][2] the Unity Package.
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
- *Token:* The token to use for AutoConnect. *Important Note: Passwords and Tokens are different things on GameJolt. You can find your token on GameJolt by clicking on your user picture then selecting `Game Token`.*

**Tip:** Leave your mouse above the option in the inspector to see its description.


### Prefab

1. Open your first scene (the first that will load).
2. Select *GameObject > Game Jolt API Manager* to add the prefab to the scene.

**Tip:** Technically it doesn't have to be the very first scene. If your scene flow is something like *splash screen > main menu > game* and that you don't call the API at all in your *splash screen* scene, you can import the prefab in the *main menu* scene. However, it is highly recommended to still import it in the very first scene so everything will be ready when you need it (i.e. trophies will be caches, the user will be automatically logged in for web players, etc.).

**Tip:** You can import the prefab in any scene for testing (so you don't have to load your game from the start every time) but remember to remove it before building your game and only leave the first one.

# Sign in

Authentication is the corner stone of the API. If you user isn't signed in, you won't have access to trophies and sessions. All you can do is post scores as guest. Let's fix that!

## Web Player and WebGL builds
For *Web Player* or *WebGL* builds hosted on Game Jolt the player will  automatically be signed in. This is how you check if a player is currently signed in or not (Guest).

```
bool isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
```

## Standalone Builds

### Default UI
For standalone builds, you have to sign the player yourself in. The easiest way is to display the default sign in form.

```
GameJolt.UI.Manager.Instance.ShowSignIn();
```

You can also pass a callback to get notified whether the player has signed in or not.

```
GameJolt.UI.Manager.Instance.ShowSignIn(
  (bool signInSuccess) => {
    Debug.Log(string.Format("Sign-in {0}", signInSuccess ? "successful" : "failed or user's dismissed the window"));
  },
  (bool userFetchedSuccess) => {
    Debug.Log(string.Format("User details fetched {0}", signInSuccess ? "successfully" : "failed"));
  }
});
```

As you can see, there are 2 callbacks. The first one returns as soon as the user is logged in, however some attributes (e.g. `user.AvatarURL`) will not be populated yet (this is also when the window is dismissed). The second one returns once all the users attributes have been populated (minus `user.Avatar` for which you have to manually call `user.DownloadAvatar()`). Depending on what you are the most interested, you will probably only subscribe to one callback like so:

```
// Get the callback as soon as the user is signed-in.
GameJolt.UI.Manager.Instance.ShowSignIn((bool signInSuccess) => { /* Do Something */ });

// Get the callback once all the user's information have been fetched.
GameJolt.UI.Manager.Instance.ShowSignIn(null, (bool userFetchedSuccess) => { /* Do Something */ });
```

**Tip:** If the manager prefab is in the current scene, you can even show this form without a single line of code! If you have a *Sign In with Game Jolt* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowSignIn ()* from the dropdown. However you cannot subscribe to callback this way.

### Custom UI
If you don't like the default UI and use some custom UI that better fit your game that's fine too! Once you've got the user name and token sign him in manually.

```
var user = new GameJolt.API.Objects.User(userName, userToken);
user.SignIn(
	(bool signInSuccess) => {
		Debug.Log(string.Format("Sign-in {0}", signInSuccess ? "successful" : "failed"));
	},
	(bool userFetchedSuccess) => {
		Debug.Log(string.Format("User details fetched {0}", signInSuccess ? "successfully" : "failed"));
	}
});
```

As you can see, there are 2 callbacks. The first one returns as soon as the user is logged in, however some attributes (e.g. `user.AvatarURL`) will not be populated yet. The second one returns once all the users attributes have been populated (minus `user.Avatar` for which you have to manually call `user.DownloadAvatar()`). Depending on what you are the most interested, you will probably only subscribe to one callback like so:

```
var user = new GameJolt.API.Objects.User(userName, userToken);

// Get the callback as soon as the user is signed-in.
user.SignIn((bool signInSuccess) => { /* Do Something */ });

// Get the callback once all the user's information have been fetched.
user.SignIn(null, (bool userFetchedSuccess) => { /* Do Something */ });
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
GameJolt.UI.Manager.Instance.ShowTrophies();
```

**Tip:** If the manager prefab is in the current scene, you can even show this window without a single line of code! If you have a *Trophies Collection* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowTrophies ()* from the dropdown.

### Custom UI
You can query one or more trophy and display it/them yourself.

Get one specific trophy.

```
var trophyID = 26534;
GameJolt.API.Trophies.Get(trophyID, (GameJolt.API.Objects.Trophy trophy) => {
	if (trophy != null)
	{
		Debug.Log(string.Format("> {0} - {1} - {2} - {3}Unlocked", trophy.Title, trophy.ID, trophy.Difficulty, trophy.Unlocked ? "" : "Not "));
	}
});
```

Get multiple trophies.

```
var trophyIDs = new int[] { 68935, 26534, 34235 };
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

Take a look at the [documentation][1] to find more ways to query trophies.

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

**Tip:** *Table ID* and *Extra Data* are optional. The former will default to the *primary* table as defined on your dashboard, the later will just be blank. This means that if you don't need the callback, you can add a score really simply `GameJolt.API.Scores.Add(scoreValue, scoreText);`

## GetRank
You can get a score value rank easily.

```
int scoreValue = 10;
int tableID = 0;

GameJolt.API.Scores.GetRank(scoreValue, tableID, (int rank) => {
	Debug.Log(string.Format("Rank {0}", rank));
});
```

## Show
### Default UI
Show all the highscore tables with a single line of code.

```
GameJolt.UI.Manager.Instance.ShowLeaderboards();
```

**Tip:** If the manager prefab is in the current scene, you can even show this window without a single line of code! If you have a *Leaderboards* button for example, in the *On Click* event listener field, drag the *GameJoltAPI > UI* and select *Manager > ShowLeaderboards ()* from the dropdown.

### Custom UI
Have a look at the [documentation][1] to see how to query *highscore tables* and *scores* to display them yourself.

# Sessions

In order for session information (like *average session length*) to be available on your dashbaord, as soon as a user is signed in, you need to create a new session and then ping it on a regular basis. Luckily, the *Unity API* does it for you, just remember to enable **Auto Ping** in the *API settings* (and make sure to turn it off if you want to handle it yourself).

Check the [documentation][1] to manage sessions manually.

# Data Store

Game Jolt allows you to save data in the cloud like game save (so the user can continue to play from another computer) or user generated content (community levels \*hoop\* \*hoop\*).

The data store function as a key/value store. The key is the name under which the data will be saved (like a variable in your code) and the value holds the value (who would have though ahah?). Be awayre that even if you store numerical values, you need to convert them as string before saving them.

Each key can either be global or belong to a specific user. Only the later require the user to be signed in.

**Note:** *Game Jolt Game API* recently added a third case where the data is global but can only be modified by the user who initially created it. This is not supported by the *Unity API* just yet but is of course on the road map.

## Set data

```
string key = "hp";
string value = Player.health.toString(); // Player is an imaginary script.
bool isGlobal = False;

GameJolt.API.DataStore.Set(key, value, isGlobal, (bool success) => {});
```

## Get data

```
string key = "hp";
bool isGlobal = False;

GameJolt.API.DataStore.Get(key, isGlobal, (string value) => {
	if (value != null)
	{
		Player.health = float.Parse(value);
	}
});
```

## More

Check the [documentation][1] to find out how to:

- Remove keys.
- Update string data with *append/prepend* operations or numerical data with *add/subtract/multiply/delete* operations.
- Retrieve all the saved keys.

# Notifications

You can use the *Unity API* notification system to display custom notifications.

```
// Text only
GameJolt.UI.Manager.Instance.QueueNotification("GameJolt is awesome!");

// Text & Image (UnityEngine.Sprite)
GameJolt.UI.Manager.Instance.QueueNotification("GameJolt is awesome!", image);
```

# Time

If your game uses timers (e.g. free gift every 24h), you might not be able to use the computer clock because the user might manipulate it. Instead, you want to fetch the server's time.

```
GameJolt.API.Misc.GetTime((System.DateTime serverTime) => {
	Debug.Log(string.Format("Server Time: {0}", time));
});
```

# What else?
It really just is a getting started tutorial. Have a look at the [documentation][1] to see what else you can do.

Also consider having a look at the [test scenes](https://github.com/loicteixeira/gj-unity-api/tree/master/Assets/Tests), especially the `console` one. While they aren't the easiest to digest, they provide a good example on how to use everything.
