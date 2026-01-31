using Godot;

namespace CosmicDoom.Scripts.Objects.Projectiles;
using static Godot.GD;

public partial class Rocket : Projectile
{
    public override void _Ready() {

    }

    public override void _Process(double delta) {
        GlobalPosition += Velocity * (float)delta;
    }
    
    public override Rocket Spawn() {
        return (Rocket)Load<PackedScene>("res://Scenes/Objects/Projectiles/rocket.tscn").Instantiate();
    }
}
