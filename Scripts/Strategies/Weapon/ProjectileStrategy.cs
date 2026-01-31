using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;

public class ProjectileStrategy(IProjectile projectile, float projectileVelocity) : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;

        var newProjectile = projectile.Spawn();
        
        ray.GlobalTransform = originalGlobalTransform;
        ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis));

        newProjectile.SetContext(context);
        ray.GetTree().Root.AddChild((Node3D)newProjectile);

        ((Node3D)newProjectile).GlobalTransform = ray.GlobalTransform;
        ray.GlobalTransform = originalGlobalTransform;
    }
}
