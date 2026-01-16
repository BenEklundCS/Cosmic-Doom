using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerLookStrategy : IControllerStrategy {
    public void Execute(InputEvent @event, IControllable controllable) {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;
        if (@event is InputEventMouseMotion motion) {
            controllable.Look(motion.Relative);
        }
    }
}