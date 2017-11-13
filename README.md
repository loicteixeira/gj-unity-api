# ‼️ Project has moved 🚨

*It is now maintained at [InfectedBytes/gj-unity-api](https://github.com/InfectedBytes/gj-unity-api/).*

# Game Jolt Unity API
Game Jolt [Game API](http://gamejolt.com/api/doc/game/) wrapper for [Unity](http://unity3d.com/).

Check out the [documentation](http://loicteixeira.github.io/gj-unity-api/).

## License
Released under [MIT](https://raw.githubusercontent.com/loicteixeira/gj-unity-api/master/LICENSE.txt).

## Development

### Changelog

Keeping `/CHANGELOG.md` up to date as new features are added make the release of new versions much simpler.

In addition, it gives a clear indication on what has been added/fixed/etc but not released yet.

### Documentation

The documentation is generated with [Doxygen](https://www.stack.nl/~dimitri/doxygen/index.html) and can be used via the GUI or the CLI.

The configuration file can be found at `/Documentation/Doxyfile`. Unfortunately the paths are absolute and will depend on the developer machine.

To update the documentation, run `make doc_generate`. To preview it, run `make doc_serve` and visit `http://localhost:8000/`. Finally, to publish the documentation, `commit` the changes to `master` and `push` to `origin`, then run `make doc_publish` to copy the `Documentation/Output/html` subdirectory to the `gh-pages` branch so the documentation will be available at [loicteixeira.github.io/gj-unity-api/](http://loicteixeira.github.io/gj-unity-api/).

### Release new version

#### Part 1 - Prepare the package
1. Update the version number following [Semantic Versioning](http://semver.org/).
    - `VERSION` in `/Assets/Plugins/GameJolt/Scripts/API/Constants.cs`.
   - `PROJECT_NUMBER` in `/Documentation/Doxyfile`.
1. Update `CHANGELOG.md`.
    - Convert the `Unreleased` section to the new version number, following the formatting of the previous versions.
    - Make sure to create the corresponding link at the bottom of the file
    - Make sure to update the `unreleased` link as well to it compare from the new version.
1. Commit the changes with the message `Release vX.Y.Z`.
1. Push the changes to the remote. See the [2.2.0 release](https://github.com/loicteixeira/gj-unity-api/commit/d8eef72a2619ae6e07d10e91c262e32535630d59#diff-4ac32a78649ca5bdd8e0ba38b7006a1e) as an example.
1. Create the corresponding tag with `git tag vX.Y.Z`.
1. Push the tag to the remote with `git push --tags`

#### Part 2 - Create the Unity Package
1. In the `Project` view, select the `Plugins` folder.
1. Right-click and choose `Assets > Export Package...`.
1. Uncheck `include dependencies`.
1. Click `Export`.
1. Save as `GameJoltUnityAPI_X.Y.Z.unitypackage` in a folder which isn't tracked by git.

#### Part 3 - Upload the package
1. GitHub
    1. Go to `Code > Releases`.
    1. Click `Draft a new Release`.
    1. In `tag` and `version`, use the name of the tag created earlier. It should be `vX.Y.Z`.
    1. Copy the changes from the changelog in the body (formatting can stay the same)
    1. Attach the newly created package.
    1. Click `Publish release`
1. GameJolt
    1. Go to `Dashboard > Your Games > Unity API > Manage`.
    1. Select `Packages`.
    1. Find `Unity API v2` and click `Manage`.
    1. Click `New Release`.
    1. Set the version number.
    1. Attach the build as a `Downloadable Build`.
    1. Select `Other` for the `platform`.
    1. Publish.
    1. Create a news in the `Devlog` so the developers following the project are notified.
1. Unity Asset Store
