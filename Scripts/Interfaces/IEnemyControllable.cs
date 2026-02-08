namespace CosmicDoom.Scripts.Interfaces;

using Godot;

public interface IEnemyControllable {
    public void MoveTo(Vector3 position);
    public void FaceTarget(Vector3 position);
    public void Attack();
    public bool CanAttack();
}
