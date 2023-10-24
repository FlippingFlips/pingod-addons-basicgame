# PinGod-AddOns-BasicGame
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![Godot Engine](https://img.shields.io/badge/GODOT-%23FFFFFF.svg?style=for-the-badge&logo=godot-engine) 

This is a modification of the demo from [pingod-addons](pingod-addons).
This is to demonstrate how to make basic game with more switches, modes from the existing repoistory.

*This requires the `addons` directory to build and run.*

---
## Autoload Scene Overrrides

|Scene|Changes
|--|--|
[Machine.tscn](autoload/Machine.tscn) | Added more switches
[Resources.tscn](autoload/Resources.tscn) | Added ball save scene to pre load
[WindowActions.tscn](autoload/WindowActions.tscn) | To enable/disable switch window without touching addons scenes
[PinGodGame.tscn](autoload/PinGodGame.tscn) | Uses a custom `PinGodGame.cs` script named `CustomPinGodGame.cs` to demonstrate overriding methods, creating custom players.

---
## Added Scenes

---
### basicgame/MainScene.tscn (starting scene)
This is a copy of the `MainScene` from the addons. It reuses the script `MainScene.cs` with no changes as this just handles mostly adding the Game, Attract, Service scenes

In the godot scene inspector we added the path to our custom `basicgame/Game.tscn` in the scenes `Game Scene Path`.
This will load our game scene instead of the default.

---
### basicgame/Game.tscn Scene
This is a copy of the `Game.tscn` from the addons.
This scene uses a custom class `scripts/BasicGame.cs` instead of the default `Game.cs`.

In the `Modes` CanvasLayer we have added the `scenes/BasicMode.tscn` scene.

---
### BaseMode Scene
This mode handles displaying a `BallSave` scene when ball saved, handles the other switches to add points and the saucer to trigger a multiball.

A `Saucer` from the controls is added to this scene tree which starts the `multiball`.

---
## Simulators
As in the `pingod-addons` demo it is loaded the same way.