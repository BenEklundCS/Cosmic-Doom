namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Interfaces;

public interface IBackgroundAction {
    void Execute(IEnemyControllable enemy, double delta);
}
