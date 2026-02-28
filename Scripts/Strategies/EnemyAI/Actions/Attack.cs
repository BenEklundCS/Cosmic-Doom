using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions;

public class Attack : IAction {
    public float Score(IEnemyControllable enemy) => enemy.CanAttack() ? 1f : 0f;

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy.CanAttack()) enemy.Attack();
    }
}