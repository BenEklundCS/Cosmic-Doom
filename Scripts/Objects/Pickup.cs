using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Registry;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Items;

public partial class Pickup : CharacterBody3D, ISpawnable {
    [Signal] public delegate void ConsumedEventHandler();
    [Export] public PickupType Type;
    public PickupCategory Category;

    private Sprite3D _sprite;

    public override void _Ready() {
        var pickupData = PickupRegistry.INSTANCE.Get(Type);
        Category = pickupData.CATEGORY;
        _sprite = GetNode<Sprite3D>("Sprite3D");
        _sprite.Texture = pickupData.TEXTURE;
    }

    public Node3D Spawn() {
        return (Pickup)Load<PackedScene>("res://Scenes/Objects/pickup.tscn").Instantiate();
    }

    public void Consume() {
        EmitSignalConsumed();
        QueueFree();
    }
}
