namespace CosmicDoom.Scripts.Objects.Projectiles;

using Godot;
using static Godot.GD;
using Interfaces;
using Context;

public partial class Projectile : Node3D, IProjectile<Projectile> {
    private RAttackContext _context;
    protected Vector3 Velocity;
    
    public void SetContext(RAttackContext context) {
        _context = context;
    }

    public void SetVelocity(Vector3 velocity) {
        Velocity = velocity;
    }

    public virtual Projectile Spawn() {
        return null;
    }
    
    IProjectile IProjectile.Spawn() => Spawn();
}
