using CosmicDoom.Scripts.Entities;
using Godot;

namespace CosmicDoom.Scripts.Objects.Projectiles;
using static Godot.GD;

public partial class Rocket : Projectile {
    private Area3D _collisionBox;
    private Area3D _explosionBox;
    public override void _Ready() {
        _collisionBox = GetNode<Area3D>("CollisionBox");
        _collisionBox.BodyEntered += OnBodyEntered;

        _explosionBox = GetNode<Area3D>("ExplosionBox");
    }

    public override void _Process(double delta) {
        GlobalPosition += Velocity * (float)delta;
    }
    
    public override Rocket Spawn() {
        return (Rocket)Load<PackedScene>("res://Scenes/Objects/Projectiles/rocket.tscn").Instantiate();
    }

    private void OnBodyEntered(Node3D body) {
        if (body is Player) return;
        Explode();
    }

    private void Explode() {
        Print("Collided!");

        var bodies = _explosionBox.GetOverlappingBodies();
        foreach (var body in bodies) {
            if (body is Player) continue;
            if (body is Enemy enemy) enemy.Hit(Context.WEAPON.DAMAGE);
        }
        
        QueueFree();
    }
}
