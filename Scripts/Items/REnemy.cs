using System;

namespace CosmicDoom.Scripts.Items;

using Godot;
using Strategies;

public enum EnemyType {
    Destroyer,
    Turret,
    Spider,
    Ender,
    Exploder,
    PlasmaBot,
    Warrior,
}

public record REnemy(
    EnemyType TYPE,
    SpriteFrames SPRITE_FRAMES,
    Func<IEnemyAiStrategy> STRATEGY,
    WeaponType WEAPON_TYPE,
    PickupType[] PICKUPS,
    AudioStreamWav ON_HIT_SOUND,
    AudioStreamWav ON_DEATH_SOUND,
    AudioStreamWav ON_RECOGNITION_SOUND
);
