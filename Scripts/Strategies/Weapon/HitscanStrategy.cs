namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;
using Interfaces;

public class HitscanStrategy(
    int shotCount = 1,
    float spreadDegrees = 0.0f
) : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var damage = context.WEAPON.DAMAGE;
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;

        for (int i = 0; i < shotCount; i++) {
            var spreadQuaternion = Utils.INSTANCE.GetSpreadQuaternion(spreadDegrees);

            ray.GlobalTransform = originalGlobalTransform;
            ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis) * spreadQuaternion);

            var collider = ray.GetCollider();
            if (collider is IHittable hittable) hittable.Hit(damage, context.ATTACKER);
        }

        ray.GlobalTransform = originalGlobalTransform;
    }
}
