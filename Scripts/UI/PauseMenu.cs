namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class PauseMenu : CanvasLayer {
    public new void Show() {
        Visible = true;
        GetTree().Paused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public new void Hide() {
        Visible = false;
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void OnResumePressed() {
        Hide();
    }

    public void OnOptionsPressed() {
        GD.Print("Options pressed (not yet implemented)");
    }

    public void OnQuitPressed() {
        Hide();
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }
}