using System.Linq;
namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

using Godot;

using Interfaces;
using Entities;

public class DestroyerActionMove : IAction {
    private float _moveTimer;
    private const float PreferredRange = 300f;

    public float Score(IEnemyControllable enemy) {
        if (!enemy.HAS_RECOGNIZED_PLAYER) return 0f;
        if (enemy.CanAttack()) return 0f;

        var dist = enemy.DISTANCE_TO_PLAYER;
        var distError = Mathf.Abs(dist - PreferredRange) / PreferredRange;
        return Mathf.Clamp(distError * 0.85f, 0f, 0.85f);
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        _moveTimer -= (float)delta;
        if (_moveTimer <= 0f) {
            enemy.MoveTo(AiUtils.GetMovePositionWherePlayerVisible(node));
            _moveTimer = Utils.INSTANCE.NextFloat(node.MoveThinkingTimeRange);
        }
    }
}