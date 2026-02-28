using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

public class DestroyerActionPanic : IAction {
    private bool _inPanic = false;

    public float Score(IEnemyControllable enemy) {
        var hp = enemy.HEALTH_PERCENT;
        if (hp >= 0.3f) {
            _inPanic = false;
            return 0f;
        }
        // Exponential ramp — spikes hard as health drops toward 0
        return Mathf.Pow(1f - (hp / 0.3f), 2f);
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (_inPanic) return;
        if (enemy is not Enemy node) return;

        enemy.MoveTo(AiUtils.GetMovePositionWhereHidden(node));
        node.TargetReached += OnTargetReached;
        _inPanic = true;
    }

    private void OnTargetReached() {
        _inPanic = false;
    }
}