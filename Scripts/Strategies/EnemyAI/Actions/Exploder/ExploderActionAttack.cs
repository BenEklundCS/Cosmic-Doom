using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

public class ExploderActionAttack : IAction {
    public float Score(IEnemyControllable enemy) {
        return 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {

    }
}