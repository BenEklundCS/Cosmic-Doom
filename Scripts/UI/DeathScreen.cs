namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class DeathScreen : CanvasLayer {
    public new void Show() {
        Visible = true;
        GetTree().Paused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void OnRestartPressed() {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    public void OnQuitPressed() {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }
}