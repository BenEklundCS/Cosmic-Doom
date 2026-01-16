using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerEscapeStrategy : IControllerStrategy {
    public void Execute(InputEvent @event, IControllable controllable) {
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}