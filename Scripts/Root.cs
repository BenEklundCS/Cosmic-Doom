using CosmicDoom.Scripts.Components;
using CosmicDoom.Scripts.Entities;
using Godot;
using static Godot.GD;

namespace fps.scripts;

public partial class Root : Node3D {
    private Timer _respawnTimer;
    private Player _player;
    private Controller _controller;
    private Transform3D _playerSpawn;
    public override void _Ready() {
        _respawnTimer = GetNode<Timer>("RespawnTimer");
        _respawnTimer.Timeout += OnRespawnTimerTimeout;
        _player = GetNode<Player>("Controller/Player");
        _playerSpawn = _player.GlobalTransform;
        _player.OnDeath += OnPlayerDeath;
        _controller = GetNode<Controller>("Controller");
    }

    public override void _Process(double delta) {
        
    }

    private void OnPlayerDeath() {
        _respawnTimer.Start();
    }

    private void OnRespawnTimerTimeout() {
        var newPlayer = (Player)Load<PackedScene>("res://Scenes/Entities/player.tscn").Instantiate();
        _controller.AddChild(newPlayer);
        newPlayer.GlobalTransform = _playerSpawn;
        _controller.SetTarget(newPlayer);
        newPlayer.OnDeath += OnPlayerDeath;
    }
}