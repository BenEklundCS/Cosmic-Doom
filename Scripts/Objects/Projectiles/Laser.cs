namespace CosmicDoom.Scripts.Objects.Projectiles;
using Godot;
using static Godot.GD;

public partial class Laser : Projectile {
    private Timer _timer;
    [Export] public float Duration = 2.0f;

    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout() {
        QueueFree();
    }
    
    public override Laser Spawn() {
        return (Laser)Load<PackedScene>("res://Scenes/Objects/Projectiles/laser.tscn").Instantiate();
    }
}