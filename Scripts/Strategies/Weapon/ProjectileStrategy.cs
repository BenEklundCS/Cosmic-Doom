using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Objects.Projectiles;

namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;

public class ProjectileStrategy(IProjectile projectile = null, float projectileVelocity = 0.0f) : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;

        var newProjectile = projectile.Spawn();
        
        ray.GlobalTransform = originalGlobalTransform;
        ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis));

        var forward = -ray.GlobalTransform.Basis.Z;
        forward *= projectileVelocity;

        newProjectile.SetContext(context);
        newProjectile.SetVelocity(forward);
        ray.GetTree().Root.AddChild((Node3D)newProjectile);

        ((Node3D)newProjectile).GlobalTransform = ray.GlobalTransform;
        ray.GlobalTransform = originalGlobalTransform;
    }
}
