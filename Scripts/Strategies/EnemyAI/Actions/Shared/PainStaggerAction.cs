namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Shared;

using Godot;
using Entities;
using Interfaces;
using static Godot.GD;

public class PainStaggerAction : IBackgroundAction {
    private const float StaggerDuration = 0.5f;
    private float _painChance;
    private float _lastCheckTime = -10f;

    public PainStaggerAction(float painChance = 0.5f) {
        _painChance = Mathf.Clamp(painChance, 0f, 1f);
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        var currentTime = (float)(Time.GetTicksMsec() / 1000.0);

        // Check if we just got hit (within ~1 frame)
        if (node.LastHitTime > _lastCheckTime && currentTime - node.LastHitTime < 0.02f) {
            if (Randf() < _painChance) {
                node.SetStagger(StaggerDuration);
            }
            _lastCheckTime = currentTime;
        }
    }
}