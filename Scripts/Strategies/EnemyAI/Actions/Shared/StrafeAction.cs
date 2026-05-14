using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Shared;

using Godot;
using Entities;
using Interfaces;

public class StrafeAction : IAction {
    private const float RecentHitThreshold = 2.0f;

    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0f;
        var currentTime = (float)(Time.GetTicksMsec() / 1000.0);
        var timeSinceHit = currentTime - node.LastHitTime;
        return timeSinceHit < RecentHitThreshold ? 0.8f : 0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        var points = node.GetTree().GetNodesInGroup("points").Cast<Point>().ToArray();
        var player = node.NEAREST_PLAYER;

        if (player == null || points.Length == 0) return;

        // Pick a random visible point to strafe to
        var visiblePoints = points.Where(point => {
            var result = Utils.INSTANCE.IntersectRayOnPath(point.GlobalPosition, player.GlobalPosition);
            return result.Count > 0 && (Node)result["collider"] is Player;
        }).ToArray();

        if (visiblePoints.Length == 0) return;

        var strafeTarget = Utils.INSTANCE.RandomElement(visiblePoints).GlobalPosition;
        enemy.MoveTo(strafeTarget);
    }
}