using System;
using Godot;
using CosmicDoom.Scripts.Entities;

namespace CosmicDoom.Scripts.Levels;

public static class LevelTriggers {
    public static void WhenGroupDead(this Node caller, StringName group, Action action) {
        var nodes = caller.GetTree().GetNodesInGroup(group);
        var remaining = 0;

        foreach (var node in nodes) {
            if (node is not Character character) continue;
            remaining++;
            character.OnDeath += () => {
                remaining--;
                if (remaining == 0) action();
            };
        }

        if (remaining == 0) action();
    }
}
