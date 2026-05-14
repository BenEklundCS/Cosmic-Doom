using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using static Godot.GD;
using Context;

public class MeleeStrategy : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var damage = context.WEAPON.DAMAGE;
        var ray = context.RAY;
        var attacker = context.ATTACKER;
        var collider = ray.GetCollider();
        const float meleeRange = 3.0f;

        if (collider is IHittable && collider is Character hittableCharacter) {
            if (attacker.GlobalPosition.DistanceTo(hittableCharacter.GlobalPosition) <= meleeRange) {
                hittableCharacter.Hit(damage, attacker);
            }
        }
    }
}
