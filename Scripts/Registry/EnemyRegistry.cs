using CosmicDoom.Scripts.Strategies.EnemyAI.Actions;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Boss;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Ender;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Exploder;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.PlasmaBot;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Spider;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Turret;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Warrior;

namespace CosmicDoom.Scripts.Registry;

using Godot;
using System.Collections.Generic;
using Interfaces;
using Items;
using Strategies.EnemyAI;

using static Godot.GD;

public partial class EnemyRegistry : Node, IRegistry<EnemyType, REnemy> {
    private readonly Dictionary<EnemyType, REnemy> _registry = new() {
        [EnemyType.Destroyer] = new REnemy(
            EnemyType.Destroyer,
            GetSpriteFrames(EnemyType.Destroyer),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new DestroyerActionMove(),
                    new Attack(),
                    new DestroyerActionPanic()
                );
            },
            WeaponType.PlasmaGun,
            [PickupType.Plasma, PickupType.Armor, PickupType.Health],
            Load<AudioStreamWav>("res://Sounds/Guns/Bullet/Bullet Hit Heavy Armor 001.wav"),
            Load<AudioStreamWav>("res://Sounds/Enemies/Sci-Fi/Robotic/Robotic Frustration.wav"),
            Load<AudioStreamWav>("res://Sounds/Enemies/Sci-Fi/Robotic/Robotic Surprise.wav")
        ),
        [EnemyType.Turret] = new REnemy(
            EnemyType.Turret,
            GetSpriteFrames(EnemyType.Turret),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new TurretActionAttack()
                );
            },
            WeaponType.PlasmaGun,
            [PickupType.Plasma, PickupType.Health],
            null,
            null,
            null
        ),
        [EnemyType.Spider] = new REnemy(
            EnemyType.Spider,
            GetSpriteFrames(EnemyType.Spider),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new SpiderActionMove(),
                    new SpiderActionJumpAway()
                );
            },
            WeaponType.RocketLauncher,
            [PickupType.Rockets, PickupType.Armor],
            null,
            null,
            null
        ),
        [EnemyType.Ender] = new REnemy(
            EnemyType.Ender,
            GetSpriteFrames(EnemyType.Ender),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new EnderActionMove()
                );
            },
            WeaponType.PlasmaGun,
            [PickupType.Health, PickupType.Plasma],
            null,
            null,
            null
        ),
        [EnemyType.Exploder] = new REnemy(
            EnemyType.Exploder,
            GetSpriteFrames(EnemyType.Exploder),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new ExploderActionAttack(),
                    new ExploderActionMove()
                );
            },
            WeaponType.None,
            [PickupType.Armor, PickupType.Health],
            null,
            null,
            null
        ),
        [EnemyType.PlasmaBot] = new REnemy(
            EnemyType.PlasmaBot,
            GetSpriteFrames(EnemyType.PlasmaBot),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new PlasmaBotActionMove()
                );
            },
            WeaponType.PlasmaGun,
            [PickupType.Plasma, PickupType.Armor],
            null,
            null,
            null
        ),
        [EnemyType.Warrior] = new REnemy(
            EnemyType.Warrior,
            GetSpriteFrames(EnemyType.Warrior),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new WarriorActionMove()
                );
            },
            WeaponType.Knife,
            [PickupType.Health],
            null,
            null,
            null
        ),
        [EnemyType.Boss] = new REnemy(
            EnemyType.Boss,
            GetSpriteFrames(EnemyType.Boss),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new BossActionCharge(),
                    new BossActionRangedAttack(),
                    new BossActionMove()
                );
            },
            WeaponType.PlasmaGun,
            [PickupType.Plasma, PickupType.Armor, PickupType.Health],
            Load<AudioStreamWav>("res://Sounds/Guns/Bullet/Bullet Hit Heavy Armor 001.wav"),
            Load<AudioStreamWav>("res://Sounds/Enemies/Sci-Fi/Robotic/Robotic Frustration.wav"),
            Load<AudioStreamWav>("res://Sounds/Enemies/Sci-Fi/Robotic/Robotic Surprise.wav")
        )
    };

    public static EnemyRegistry INSTANCE { get; private set; }

    public REnemy Get(EnemyType key) {
        return _registry.GetValueOrDefault(key);
    }

    public IEnumerable<EnemyType> GetKeys() {
        return _registry.Keys;
    }

    public override void _Ready() {
        INSTANCE = this;
    }

    private static SpriteFrames GetSpriteFrames(EnemyType enemyType) {
        var spriteFrames = new SpriteFrames();
        spriteFrames.RemoveAnimation("default");
        spriteFrames.AddAnimation("idle");
        spriteFrames.AddAnimation("walk");
        spriteFrames.AddAnimation("attack");

        // Boss uses Destroyer sprites (no dedicated boss sprite assets)
        var enemyName = enemyType == EnemyType.Boss ? "destroyer" : enemyType.ToString().ToLower();

        var monstersPath = "res://Assets/Sprites/Monsters";

        foreach (var animationName in spriteFrames.GetAnimationNames()) {
            var path = $"{monstersPath}/monster_{enemyName}_{animationName}.png";
            if (!ResourceLoader.Exists(path)) continue;
            spriteFrames.AddFrame(animationName, Utils.LoadTrimmed(path));
        }
        
        spriteFrames.AddAnimation("dying");
        spriteFrames.SetAnimationLoop("dying", false);
        spriteFrames.AddFrame("dying", Utils.LoadTrimmed($"{monstersPath}/monster_dying.png"));
        spriteFrames.AddFrame("dying", Utils.LoadTrimmed($"{monstersPath}/monster_dead.png"));
        spriteFrames.AddFrame("dying", Utils.LoadTrimmed($"{monstersPath}/enemy_dead.png"));

        return spriteFrames;
    }
}
