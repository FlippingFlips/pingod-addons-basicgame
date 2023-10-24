using PinGod.Base;
using PinGod.Core;
using PinGod.Game;

/// <summary>
/// BasicGame with Modes tree (game/Game.tscn). This does nothing itself here but is loaded from Game.tscn <para/>
/// this.GetTree().Root.Connect("gui_focus_changed", this, "gui_focus_changed"); 
/// </summary>
public partial class BasicGame : Game
{
    /// <summary> Add 100 extra points to bonus </summary>
    /// <param name="points"></param>
    /// <param name="bonus"></param>
    public override void AddPoints(int points, int bonus = 25) => base.AddPoints(points, bonus + 1000);

    /// <summary> Just logs </summary>
    public void OnBallSaveDisabled() => Logger.Log(LogLevel.Info, Logger.BBColor.green, nameof(BasicGame), ":" + nameof(OnBallSaveDisabled));

    /// <summary> Just logs </summary>
	public void OnBallSaveEnabled() => Logger.Log(LogLevel.Info, Logger.BBColor.green, nameof(BasicGame), ":" + nameof(OnBallSaveEnabled));
}
