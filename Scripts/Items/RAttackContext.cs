using Godot;

namespace CosmicDoom.Scripts.Items;

using Entities;

public record RAttackContext(
    RayCast3D RAY,
    RWeapon WEAPON,
    Character ATTACKER
);