using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerScrollStrategy() : IControllerStrategy {
    public void Execute(InputEvent @event, IControllable controllable) {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;
        if (@event.IsActionPressed("wheel_up"))
            controllable.NextWeapon();
        else if (@event.IsActionPressed("wheel_down")) {
            controllable.PrevWeapon();
        }
    }
}