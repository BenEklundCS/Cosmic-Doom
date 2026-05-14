using CosmicDoom.Scripts.Effects;
using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Registry;

using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Exploder;

public class ExploderActionAttack : IAction {
    private const float EXPLOSION_THRESHOLD = 3.0f;
    private const int EXPLOSION_DAMAGE = 50;

    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0.0f;
        return node.DISTANCE_TO_PLAYER < EXPLOSION_THRESHOLD ? 1.0f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        // Deal damage to player
        if (node.NEAREST_PLAYER != null) {
            node.NEAREST_PLAYER.Hit(EXPLOSION_DAMAGE, node);
        }

        // Spawn explosion VFX at Exploder position
        EffectProvider.INSTANCE.SpawnEffectAt(EffectType.Explosion, node.GlobalPosition);

        // Suicide bomb — remove self
        node.QueueFree();
    }
}