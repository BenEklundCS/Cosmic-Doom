using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Boss;

public class BossActionCharge : IAction {
    private const float HP_THRESHOLD = 0.4f;

    public float Score(IEnemyControllable enemy) {
        return enemy.HEALTH_PERCENT < HP_THRESHOLD ? 1.0f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        // Relentless melee charge when low health
        if (node.NEAREST_PLAYER != null) {
            enemy.MoveTo(node.NEAREST_PLAYER.GlobalPosition);
        }
    }
}