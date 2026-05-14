namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Entities;
using Interfaces;
using static Godot.GD;

public class UtilityAiStrategy(IBackgroundAction[] background, params IAction[] actions) : IEnemyAiStrategy {
    public void Execute(IEnemyControllable enemy, double delta) {
        foreach (var bg in background) {
            bg.Execute(enemy, delta);
        }

        // If enemy is staggered from pain, don't execute any actions
        if (enemy is Enemy { IsStaggered: true }) {
            return;
        }

        IAction best = null;
        var bestScore = float.MinValue;

        foreach (var action in actions) {
            var score = action.Score(enemy);
            // Add ±0.025 jitter to score
            score += (float)(Randf() * 0.05f - 0.025f);
            if (score > bestScore) {
                bestScore = score;
                best = action;
            }
        }

        best?.Execute(enemy, delta);
    }
}
