using System;

namespace CosmicDoom.Scripts.Items;

using Godot;
using Strategies;

public enum EnemyType {
    Destroyer
}

public record REnemy(
    EnemyType TYPE,
    SpriteFrames SPRITE_FRAMES,
    Func<IEnemyAiStrategy> STRATEGY,
    WeaponType WEAPON_TYPE
);
