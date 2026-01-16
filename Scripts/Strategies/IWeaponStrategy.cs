using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Strategies;

public interface IWeaponStrategy {
    #nullable enable
    public Node? Execute(RAttackContext context);
}