namespace CosmicDoom.Scripts.Interfaces;

using Godot;

public interface IHittable {
    public void Hit(int damage);
    [Signal] public delegate void OnDeathEventHandler();
}