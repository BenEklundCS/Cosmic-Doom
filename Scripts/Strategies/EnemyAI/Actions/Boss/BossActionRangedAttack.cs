using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Boss;

public class BossActionRangedAttack : IAction {
    private const float MELEE_THRESHOLD = 2.5f;

    public float Score(IEnemyControllable enemy) {
        return enemy.CanAttack() && enemy.DISTANCE_TO_PLAYER > MELEE_THRESHOLD ? 1.0f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }
}