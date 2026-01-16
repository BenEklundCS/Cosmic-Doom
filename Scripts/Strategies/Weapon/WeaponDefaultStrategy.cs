using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;

namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using static Godot.GD;

public partial class WeaponDefaultStrategy : IWeaponStrategy {
    public Node Execute(RAttackContext context) {
        var laser = (Laser)Load<PackedScene>("res://Scenes/Objects/laser.tscn").Instantiate();
        laser.SetContext(context);
        var collider = context.RAY.GetCollider();
        Print(collider);
        if (collider is IHittable hittable) hittable.Hit(context.WEAPON.DAMAGE);
        return laser;
    }
}