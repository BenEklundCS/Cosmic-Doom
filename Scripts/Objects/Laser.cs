using CosmicDoom.Scripts.Items;
using Godot;

public partial class Laser : Node3D {
    private Timer _timer;
    [Export] public float Duration = 2.0f;
    
    private RAttackContext _context;

    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        
        var forward = -_context.RAY.GlobalTransform.Basis.Z;
        GlobalPosition = _context.RAY.GlobalPosition;
        LookAt(GlobalPosition + forward, Vector3.Up);
    }

    public void SetContext(RAttackContext context) {
        _context = context;
    }

    private void OnTimerTimeout() {
        QueueFree();
    }
}