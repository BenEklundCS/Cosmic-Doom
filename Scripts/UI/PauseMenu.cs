namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class PauseMenu : CanvasLayer {
	private OptionsPanel _optionsPanel;
	private CenterContainer _optionsContainer;

	public override void _Ready() {
		_optionsContainer = GetNode<CenterContainer>("OptionsCenterContainer");
		_optionsPanel = _optionsContainer.GetNode<OptionsPanel>("OptionsPanel");
	}

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

	public override void _Input(InputEvent @event) {
		if (Visible && @event.IsActionPressed("ui_cancel")) {
			Hide();
			GetViewport().SetInputAsHandled();
		}
	}

	public void OnResumePressed() {
		Hide();
	}

	public void OnOptionsPressed() {
		_optionsPanel.ShowPanel();
	}

	public void OnQuitPressed() {
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
