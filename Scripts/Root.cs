using CosmicDoom.Scripts.Entities;
using Godot;
using static Godot.GD;

namespace fps.scripts;

public partial class Root : Node3D {
    private Timer _respawnTimer;
    private Player _player;
    private Transform3D _playerSpawn;
    public override void _Ready() {
        _respawnTimer = GetNode<Timer>("RespawnTimer");
        _respawnTimer.Timeout += OnRespawnTimerTimeout;
        _player = GetNode<Player>("Player");
        _playerSpawn = _player.GlobalTransform;
        _player.OnDeath += OnPlayerDeath;
    }

    public override void _Process(double delta) {

    }

    private void OnPlayerDeath() {
        _respawnTimer.Start();
    }

    private void OnRespawnTimerTimeout() {
        var newPlayer = (Player)Load<PackedScene>("res://Scenes/Entities/player.tscn").Instantiate();
        AddChild(newPlayer);
        newPlayer.GlobalTransform = _playerSpawn;
        newPlayer.OnDeath += OnPlayerDeath;
    }
}