namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class WinScreen : CanvasLayer {
    public new void Show() {
        Visible = true;
        GetTree().Paused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void OnMenuPressed() {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }
}