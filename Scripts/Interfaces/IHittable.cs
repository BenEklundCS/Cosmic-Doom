using Godot;

namespace CosmicDoom.Scripts.Interfaces;

public interface IHittable {
    [Signal]
    public delegate void OnDeathEventHandler();

    public void Hit(int damage);
}