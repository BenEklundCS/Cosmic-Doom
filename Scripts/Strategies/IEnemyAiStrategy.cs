namespace CosmicDoom.Scripts.Strategies;

using Entities;

public interface IEnemyAiStrategy {
    public void Execute(Enemy enemy, double delta);
}
