using Godot;
using PinGod.Core;
using PinGod.Game;
using PinGod.Modes;

/// <summary> Base mode example. This mode "control" is added to the Game scene <para/>
/// This mode hooks onto the <see cref="OnBallSaved"/> which is triggered by the game if the mode belongs to the mode group <para/>
/// Processes switches from the pinGod.MachineNode.SwitchCommand. Slingshots, bumpers, lanes
/// </summary>
public partial class BaseMode : Control
{
	#region Fields
	private Saucer _ballSaucer;
	private PackedScene _ballSaveScene;
	private Game game;
	private IPinGodGame pinGod;
	#endregion

	#region Exports
	/// <summary> Scene to play when ball save becomes active </summary>
	[Export(PropertyHint.File)] string BALL_SAVE_SCENE;
	#endregion

	#region Godot Overrides
	/// <summary>Gets access to <see cref="PinGodGame"/> and the main <see cref="Game"/> scene.</summary>
	public override void _EnterTree()
	{
		game = GetParent().GetParent() as BasicGame;

		//add a ball save scene to instance if available in resources
		if (!string.IsNullOrEmpty(BALL_SAVE_SCENE))
		{
            var resources = GetNodeOrNull<Resources>(Paths.ROOT_RESOURCES);
            if (resources != null)
            {
                _ballSaveScene = resources.GetPackedSceneFromResource(BALL_SAVE_SCENE);    
			}
        }		

		//set the ball save packed scene
		if (!string.IsNullOrWhiteSpace(BALL_SAVE_SCENE)) _ballSaveScene = GD.Load<PackedScene>(BALL_SAVE_SCENE);
		else { Logger.Warning(nameof(BaseMode), ": no ball save scene set"); }

		_ballSaucer = GetNode<Saucer>(nameof(Saucer));
	}

	/// <summary> Hooks onto the PinGodGame.Machine switch handler where we process our switches</summary>
	public override void _Ready()
	{
		base._Ready();
		if (HasNode("/root/PinGodGame"))
		{
			pinGod = GetNode("/root/PinGodGame") as IPinGodGame;
			//use the switch command on machine through the game as we're in a game
			pinGod.MachineNode.SwitchCommand += OnSwitchCommandHandler;
		}
		else { Logger.WarningRich(nameof(BaseMode), "[color=red]", ": no PinGodGame found", "[/color]"); }
	}
	#endregion

	#region Mode Callbacks
	/// <summary>Not used. Can use to act when ball drains</summary>
	public void OnBallDrained() { }

	/// <summary>Displays a ball save scene for 2 seconds if not in multi-ball, <see cref="PinGodGame.IsMultiballRunning"/></summary>
	public void OnBallSaved()
	{
		if (!pinGod.IsMultiballRunning)
		{
			Logger.Debug(nameof(BaseMode), ": ball saved, no multi-ball");
			//add ball save scene to tree and remove after 2 secs;
			CallDeferred(nameof(DisplayBallSaveScene), 2f);
		}
		else
		{
			Logger.Debug(nameof(BaseMode), ":skipping save display in multi-ball");
		}
	}

	/// <summary>This is called if this mode belongs in the Mode group</summary>
	public void OnBallStarted() { }

	/// <summary>This is called if this mode belongs in the Mode group</summary>
	public void UpdateLamps() { } 
	#endregion

	/// <summary>Adds a ball save scene to the tree and removes</summary>
	/// <param name="time">removes the scene after the time</param>
	private void DisplayBallSaveScene(float time = 2f)
	{
		var ballSaveScene = _ballSaveScene?.Instantiate<BallSave>();
		if (ballSaveScene != null)
		{
			Logger.Debug(nameof(BaseMode), ": displaying ball save scene");
			ballSaveScene.SetRemoveAfterTime(time);
			AddChild(_ballSaveScene.Instantiate());
		}
		else { Logger.Debug(nameof(BaseMode), ": ball saver scene not set."); }
	}

	/// <summary>Saucer "kicker" active.</summary>
	private void OnBallStackPinball_SwitchActive()
	{
		if (!pinGod.IsTilted && pinGod.GameInPlay)
		{
			pinGod.AddPoints(150);

			if (!pinGod.IsMultiballRunning)
			{
				Logger.Debug($"{nameof(BaseMode)}:{nameof(OnBallStackPinball_SwitchActive)}", ": starting multiball");
				//enable multiball and start timer on default timeout (see BaseMode scene, BallStackPinball)
				pinGod.IsMultiballRunning = true;
				_ballSaucer.Start();

				game?.CallDeferred(nameof(BasicGame.AddMultiballSceneToTree));
				return;
			}
		}

		//no multiball running or game not in play
		_ballSaucer.Start(1f);
	}

	private void OnBallStackPinball_timeout()
	{
		Logger.Debug(nameof(BaseMode), ":ballstack timedout");
		_ballSaucer.Kick();
	}

	/// <summary>Switch handlers for lanes, slingshots and bumpers. This example uses a switch case to add points.</summary>
	/// <param name="name"></param>
	/// <param name="index"></param>
	/// <param name="value"></param>
	private void OnSwitchCommandHandler(string name, byte index, byte value)
	{
		if (value <= 0) return;
		switch (name)
		{
			case "outlaneL":
			case "outlaneR":
				game.AddPoints(250);
				break;
			case "inlaneL":
			case "inlaneR":
				game.AddPoints(100);
				break;
			case "slingL":
			case "slingR":
				game.AddPoints(50);
				break;
            case "bumper1":
            case "bumper2":
            case "bumper3":
            case "bumper4":
                game.AddPoints(25);
                break;
            default:
				break;
		}
	}
}
