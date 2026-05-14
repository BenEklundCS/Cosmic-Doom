namespace CosmicDoom.Scripts.UI;

using Godot;
using static Godot.GD;

public partial class OptionsPanel : PanelContainer {
    private HSlider _masterSlider;
    private HSlider _sfxSlider;
    private HSlider _musicSlider;
    private ConfigFile _config;
    private const string ConfigPath = "user://cosmic_doom.cfg";

    public override void _Ready() {
        _masterSlider = GetNode<HSlider>("VBoxContainer/MasterSlider");
        _sfxSlider = GetNode<HSlider>("VBoxContainer/SFXSlider");
        _musicSlider = GetNode<HSlider>("VBoxContainer/MusicSlider");

        _config = new ConfigFile();
        _config.Load(ConfigPath);

        LoadSettings();
    }

    private void LoadSettings() {
        var masterVol = (float)_config.GetValue("audio", "master", 1.0f);
        var sfxVol = (float)_config.GetValue("audio", "sfx", 1.0f);
        var musicVol = (float)_config.GetValue("audio", "music", 1.0f);

        _masterSlider.Value = masterVol;
        _sfxSlider.Value = sfxVol;
        _musicSlider.Value = musicVol;

        ApplyVolumeSettings(masterVol, sfxVol, musicVol);
    }

    public void OnMasterVolumeChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(value));
        _config.SetValue("audio", "master", value);
        _config.Save(ConfigPath);
    }

    public void OnSFXVolumeChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Mathf.LinearToDb(value));
        _config.SetValue("audio", "sfx", value);
        _config.Save(ConfigPath);
    }

    public void OnMusicVolumeChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(value));
        _config.SetValue("audio", "music", value);
        _config.Save(ConfigPath);
    }

    private bool _didPause;

    private Control _container;

    public void ShowPanel() {
        _container ??= GetParent<Control>();
        _container.Visible = true;
        if (!GetTree().Paused) {
            GetTree().Paused = true;
            _didPause = true;
        }
    }

    public void ClosePanel() {
        _container ??= GetParent<Control>();
        _container.Visible = false;
        if (_didPause) {
            GetTree().Paused = false;
            _didPause = false;
        }
    }

    public override void _Input(InputEvent @event) {
        if (_container == null || !_container.Visible) return;

        if (@event.IsActionPressed("ui_cancel")) {
            ClosePanel();
            GetViewport().SetInputAsHandled();
        } else if (@event is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex == MouseButton.Left) {
            if (!GetGlobalRect().HasPoint(mb.GlobalPosition)) {
                ClosePanel();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    public void OnClosePressed() {
        ClosePanel();
    }

    private void ApplyVolumeSettings(float master, float sfx, float music) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(master));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Mathf.LinearToDb(sfx));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(music));
    }
}