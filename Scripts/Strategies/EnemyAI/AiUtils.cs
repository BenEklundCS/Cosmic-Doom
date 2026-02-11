using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;

public class AiUtils {
    public static Vector3 GetMovePositionWherePlayerVisible(Enemy enemy) {
        var points = enemy.GetTree().GetNodesInGroup("points").Cast<Point>().ToArray();
        var player = (Player)enemy.GetTree().GetNodesInGroup("players")[0];

        if (player == null || points.Length == 0) {
            return enemy.GlobalPosition;
        }

        var pointsWherePlayerIsVisible = points.Where(point => {
            var result = Utils.INSTANCE.IntersectRayOnPath(point.GlobalPosition, player.GlobalPosition);
            return result.Count > 0 && (Node)result["collider"] is Player;
        }).ToArray();

        if (pointsWherePlayerIsVisible.Length == 0) {
            return Utils.INSTANCE.RandomElement(points).GlobalPosition;
        }

        return pointsWherePlayerIsVisible
            .MinBy(point => enemy.GlobalPosition.DistanceTo(point.GlobalPosition))!
            .GlobalPosition;
    }

    public static Vector3 GetMovePositionWhereHidden(Enemy enemy) {
        var points = enemy.GetTree().GetNodesInGroup("points").Cast<Point>().ToArray();
        var player = (Player)enemy.GetTree().GetNodesInGroup("players")[0];
        
        if (player == null || points.Length == 0) {
            return enemy.GlobalPosition;
        }
        
        var pointsWherePlayerIsNotVisible = points.Where(point => {
            var result = Utils.INSTANCE.IntersectRayOnPath(point.GlobalPosition, player.GlobalPosition);
            return result.Count > 0 && (Node)result["collider"] is not Player;
        }).ToArray();
        
        if (pointsWherePlayerIsNotVisible.Length == 0) {
            return Utils.INSTANCE.RandomElement(points).GlobalPosition;
        }
        
        return pointsWherePlayerIsNotVisible
            .MaxBy(point => enemy.GlobalPosition.DistanceTo(point.GlobalPosition))!
            .GlobalPosition;
    }
}