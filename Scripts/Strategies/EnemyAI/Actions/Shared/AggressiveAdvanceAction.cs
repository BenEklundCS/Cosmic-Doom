namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Shared;

using Godot;
using Entities;
using Interfaces;

public class AggressiveAdvanceAction : IAction {
    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0f;
        if (node.NEAREST_PLAYER == null) return 0f;

        var currentTime = (float)(Time.GetTicksMsec() / 1000.0);
        var timeSinceLastDamage = currentTime - node.LastDamageDealtTime;
        var score = 0.75f + timeSinceLastDamage * 0.05f;
        return Mathf.Min(score, 0.9f);
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;
        var target = AiUtils.GetMovePositionWherePlayerVisible(node);
        enemy.MoveTo(target);
    }
}