using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerJumpStrategy : IControllerStrategy {
    public void Execute(InputEvent @event, IControllable controllable) {
        controllable.Jump();
    }
}