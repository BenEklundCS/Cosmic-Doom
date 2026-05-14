namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class MainMenu : CanvasLayer {
    private OptionsPanel _optionsPanel;
    private CenterContainer _optionsContainer;

    public override void _Ready() {
        _optionsContainer = GetNode<CenterContainer>("OptionsCenterContainer");
        _optionsPanel = _optionsContainer.GetNode<OptionsPanel>("OptionsPanel");
    }

    public void OnPlayPressed() {
        GetTree().ChangeSceneToFile("res://Scenes/root.tscn");
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event.IsActionPressed("ui_cancel")) {
            if (_optionsPanel.Visible)
                _optionsPanel.ClosePanel();
            else
                _optionsPanel.ShowPanel();
        }
    }

    public void OnOptionsPressed() {
        _optionsPanel.ShowPanel();
    }

    public void OnQuitPressed() {
        GetTree().Quit();
    }
}