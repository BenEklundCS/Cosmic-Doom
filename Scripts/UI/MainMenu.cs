namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class MainMenu : CanvasLayer {
    public override void _Ready() {
        // Button connections are made in the scene
    }

    public void OnPlayPressed() {
        GetTree().ChangeSceneToFile("res://Scenes/root.tscn");
    }

    public void OnOptionsPressed() {
        // TODO: Show options panel
        GD.Print("Options pressed (not yet implemented)");
    }

    public void OnQuitPressed() {
        GetTree().Quit();
    }
}