namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;

public class DefaultStrategy : IEnemyAiStrategy {
    public void Execute(Enemy enemy, double delta) {
        var minDistance = float.MaxValue;
        Player nearestPlayer = null;
        FindNearestPlayer(enemy.GetTree().Root, enemy, ref minDistance, ref nearestPlayer);

        if (nearestPlayer != null) {
            var targetPos = nearestPlayer.GlobalPosition;
            enemy.MoveTo(targetPos);
            enemy.FaceTarget(targetPos);
        }

        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }

    private static void FindNearestPlayer(Node node, Enemy enemy, ref float minDistance, ref Player nearestPlayer) {
        if (node is Player player) {
            var distance = enemy.GlobalPosition.DistanceTo(player.GlobalPosition);
            if (distance < minDistance) {
                minDistance = distance;
                nearestPlayer = player;
            }
        }
        foreach (var child in node.GetChildren()) {
            FindNearestPlayer(child, enemy, ref minDistance, ref nearestPlayer);
        }
    }
}
