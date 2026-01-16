using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.Controller;

public class ControllerMoveStrategy(StringName left, StringName right, StringName up, StringName down) : IControllerStrategy {
    public void Execute(InputEvent? @event, IControllable controllable) {
        var input = Input.GetVector(left, right, up, down);
        var movement = new Vector3(input.X, 0, input.Y);
        controllable.Move(movement);
    }
}