# Change Log
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

...

##[2.1.1] (2016-06-21)

### Fixed
- Could not unlock trophies. #64
  - *Thanks to RomejanicDev for reporting the issue.*

##[2.1.0] (2016-06-19)

### Warning
- The minimum version to use the API is now **Unity 5.0.1**.
- License is now MIT (less restrictive). #62.

### Fixed
- ShowLeaderboards callback not being called. #51
  - *Thanks to @michidk for his contribution and @WizzardMaker for his help.*
- Call signature could be invalid when using URL with forward slash. #59
  - *Thanks to DerpVulpes for his contribution*

### Other
- Thanks to @andyman for helping other users on Twitter.


##[2.0.2] (2015-07-18)

### Fixed
- AutoConnect for WebGL builds was broken. #46.
  - *Thanks to David Florek for reporting the issue.*
- The UI was not on the UI layer. #49.
  - *Thanks to Piotrek for reporting the issue.*

### Deprecated
- Unity 5.0.0 has a bug regarding md5 calculations for WebGL builds. It has been fixed in Unity 5.0.1. Therefore **the API will require Unity 5.0.1 from the next update.**


##[2.0.1] (2015-07-11)

### Fixed
- Allow UI do display when Time.timeScale is set to 0.
- Automatically create EventSystem if there is none.
  - *Thanks to Gosh Darn Games for reporting the issue.*
- Prevent user to be unauthenticated if he signed in with his name with a different case as what is stored in GameJolt database.
  - *Thanks to @sebasrez for reporting the issue and providing useful information.*

##[2.0.0] (2015-07-02)

Initial release

[Unreleased]: https://github.com/loicteixeira/gj-unity-api/compare/v2.1.1...HEAD
[2.1.1]: https://github.com/loicteixeira/gj-unity-api/tree/v2.1.1
[2.1.0]: https://github.com/loicteixeira/gj-unity-api/tree/v2.1.0
[2.0.2]: https://github.com/loicteixeira/gj-unity-api/tree/v2.0.2
[2.0.1]: https://github.com/loicteixeira/gj-unity-api/tree/v2.0.1
[2.0.0]: https://github.com/loicteixeira/gj-unity-api/tree/v2.0.0

