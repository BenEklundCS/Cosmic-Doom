using CosmicDoom.Scripts.Entities;

namespace CosmicDoom.Scripts.Interfaces;

using Godot;

public interface IEnemyControllable {
    public float HEALTH_PERCENT { get; }
    public float DISTANCE_TO_PLAYER { get; }
    public bool HAS_RECOGNIZED_PLAYER { get; }
    public bool IS_MOVING { get; }
    public Player NEAREST_PLAYER { get; }
    public void MoveTo(Vector3 position);
    public void FaceTarget(Vector3 position);
    public void Attack();
    public bool CanAttack();
}
