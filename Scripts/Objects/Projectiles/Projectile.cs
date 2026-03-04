namespace CosmicDoom.Scripts.Objects.Projectiles;

using Godot;
using static Godot.GD;
using Interfaces;
using Context;

public partial class Projectile : Node3D, IProjectile<Projectile> {
    protected RAttackContext Context;
    protected Vector3 Velocity;
    protected AudioStreamPlayer3D OnHitAudio;

    public override void _Ready() {
        OnHitAudio = GetNode<AudioStreamPlayer3D>("OnHitAudio");
    }

    public void SetContext(RAttackContext context) {
        Context = context;
    }

    public void SetVelocity(Vector3 velocity) {
        Velocity = velocity;
    }

    public virtual Projectile Spawn() {
        return null;
    }
    
    IProjectile IProjectile.Spawn() => Spawn();
}
