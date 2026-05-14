using System.Linq;
namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Exploder;

using Godot;

using Interfaces;
using Entities;

public class ExploderActionMove : IAction {
    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0.0f;
        return node.HAS_RECOGNIZED_PLAYER ? 0.9f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        // Relentless advance toward player to detonate
        if (node.NEAREST_PLAYER != null) {
            enemy.MoveTo(node.NEAREST_PLAYER.GlobalPosition);
        }
    }
}