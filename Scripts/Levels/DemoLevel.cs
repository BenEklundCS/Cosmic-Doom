using System;
using Godot;
using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Levels;

public partial class DemoLevel : NavigationRegion3D {
    private static readonly PackedScene EnemyScene = GD.Load<PackedScene>("res://Scenes/Entities/enemy.tscn");
    private static readonly PackedScene BossScene = GD.Load<PackedScene>("res://Scenes/Entities/boss.tscn");
    private const float WaveDelay = 5.0f;

    private static readonly (EnemyType Type, Vector3 Pos)[] Wave2Spawns = {
        (EnemyType.Destroyer, new(-10, 0, -12)),
        (EnemyType.Destroyer, new(10, 0, -12)),
        (EnemyType.Warrior,   new(-10, 0, 12)),
        (EnemyType.Warrior,   new(10, 0, 12)),
        (EnemyType.Exploder,  new(-4, 0, 13)),
        (EnemyType.Exploder,  new(4, 0, 13)),
    };

    public override void _Ready() {
        this.WhenGroupDead("demo_wave1", () => After(WaveDelay, SpawnWave2));
    }

    private void SpawnWave2() {
        foreach (var (type, pos) in Wave2Spawns)
            SpawnEnemy(EnemyScene, type, pos, "demo_wave2");
        this.WhenGroupDead("demo_wave2", () => After(WaveDelay, SpawnWave3));
    }

    private void SpawnWave3() {
        SpawnEnemy(EnemyScene, EnemyType.Turret, new(-10, 0, -13), "demo_wave3");
        SpawnEnemy(EnemyScene, EnemyType.Turret, new(10, 0, -13), "demo_wave3");
        SpawnEnemy(BossScene, EnemyType.Boss, new(0, 0, -11), "demo_wave3");
    }

    private void After(float seconds, Action action) {
        GetTree().CreateTimer(seconds).Timeout += action;
    }

    private void SpawnEnemy(PackedScene scene, EnemyType type, Vector3 pos, StringName group) {
        var enemy = scene.Instantiate<Enemy>();
        enemy.Type = type;
        enemy.GlobalPosition = pos;
        enemy.CustomGroupName = group;
        AddChild(enemy);
    }
}