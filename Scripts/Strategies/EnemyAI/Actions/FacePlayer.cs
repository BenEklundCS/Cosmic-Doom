using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions;

using Entities;
using Interfaces;

public class FacePlayer : IBackgroundAction {
    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        var nearestPlayer = node
            .GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => node.GlobalPosition.DistanceTo(player.GlobalPosition));

        if (nearestPlayer != null) {
            enemy.FaceTarget(nearestPlayer.GlobalPosition);
        }
    }
}
