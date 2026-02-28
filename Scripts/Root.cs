using System;
using CosmicDoom.Scripts.Entities;
using Godot;
using static Godot.GD;

namespace fps.scripts;

public partial class Root : Node3D {
    [Export] public int WorldDamage = 25;
    private Timer _respawnTimer;
    private Player _player;
    private Transform3D _playerSpawn;
    private float _lowestY;
    
    public override void _Ready() {
        _respawnTimer = GetNode<Timer>("RespawnTimer");
        _respawnTimer.Timeout += OnRespawnTimerTimeout;
        _player = GetNode<Player>("Player");
        _playerSpawn = _player.GlobalTransform;
        _player.OnDeath += OnPlayerDeath;
        _lowestY = GetLowestYLevel();
    }

    public override void _Process(double delta) {
        if (_player?.GlobalPosition.Y < _lowestY - 100.0f) {
            _player?.Hit(WorldDamage);
        }
    }

    private void OnPlayerDeath() {
        _player = null;
        _respawnTimer.Start();
    }

    private void OnRespawnTimerTimeout() {
        var newPlayer = (Player)Load<PackedScene>("res://Scenes/Entities/player.tscn").Instantiate();
        AddChild(newPlayer);
        newPlayer.GlobalTransform = _playerSpawn;
        newPlayer.OnDeath += OnPlayerDeath;
        _player = newPlayer;
    }

    private float GetLowestYLevel() {
        var lowest = float.PositiveInfinity;
        foreach (var child in GetTree().Root.GetChildren()) {
            if (child is Node3D node3D) {
                var y = node3D.GlobalPosition.Y;
                lowest = Math.Min(lowest, y);
            }
        }
        return float.IsPositiveInfinity(lowest) ? 0.0f : lowest;
    }
}