using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerAttackStrategy : IControllerStrategy {
    public void Execute(InputEvent @event, IControllable controllable) {
        if (Input.MouseMode == Input.MouseModeEnum.Visible)
            Input.MouseMode = Input.MouseModeEnum.Captured;
        else
            controllable.Attack();
    }
}