@page faq FAQ

# I can't click the UI
You are probably missing the EventSystem in your scene. Manually add one to your scene (`GameObject > UI > EventSystem`) or make sure to use the API v2.0.1 or more so it is automatically added when you add the API to your scene (`GameObject > GameJolt API Manager`).

# Sign in keeps failing even with the right username and password
You should sign in with your game token, not your password (you password is only used to log into GameJolt). You can find your token on GameJolt by clicking on your user picture then selecting `Game Token`.

# The trophy notification keeps coming up
You are probably unlocking the trophy in the `Update` loop, therefore triggering it on every frame. See this [issue](https://github.com/loicteixeira/gj-unity-api/issues/68) for more details.

# AutoConnect doesn't work with WebGL
Make sure you use the API `v2.1.0` or higher and Unity `5.0.1` or higher. There is a bug in previous versions.

# How can I add a float as a score?
You can't use `floats` directly as scores are saved as integer on the server. However, if you have a fixed number of decimals, you can convert your `float` to `int` before saving it (and use the score display string to still show a `float`).

For example:
```
float score = 54.359f;
int scoreValue = (int)(score * 1000);  // Will be `54359`.
string scoreText = String.Format("Race completed in {0}s!", score); // Will be `Race completed in 54.359s!`.

GameJolt.API.Scores.Add(scoreValue, scoreText);
```