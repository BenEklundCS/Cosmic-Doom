using Godot;
using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Objects;

public partial class LevelOne : NavigationRegion3D {
    private static readonly PackedScene EnemyScene = GD.Load<PackedScene>("res://Scenes/Entities/enemy.tscn");

    private static readonly Vector3[] DestroyerSpawnPositions = {
        new(39.71f, 0.59f, 14.44f),
        new(29.96f, 0f, 7.48f),
        new(29.96f, 0f, -0.63f),
    };

    public override void _Ready() {
        var door1 = GetNode<TriggerableDoor>("Room2/TriggerableDoor2");
        var door2 = GetNode<TriggerableDoor>("Room5/TriggerableDoor");
        

        AddChild(new Observer(() => GetTree().GetNodeCountInGroup("wave1group") == 0, SpawnDestroyerWave));

        AddChild(new Observer(
            () => GetTree().GetNodeCountInGroup("extraWave1group") == 0,
                () => door1.SetDoorOpen(true)
        ));
        
        AddChild(new Observer(
            () => GetTree().GetNodeCountInGroup("door2group") == 0,
            () => door2.SetDoorOpen(true)
        ));
    }

    private void SpawnDestroyerWave() {
        foreach (var pos in DestroyerSpawnPositions) {
            var enemy = EnemyScene.Instantiate<Enemy>();
            enemy.Type = EnemyType.Destroyer;
            enemy.GlobalPosition = pos;
            enemy.CustomGroupName = "extraWave1group";
            AddChild(enemy);
        }
    }
}
