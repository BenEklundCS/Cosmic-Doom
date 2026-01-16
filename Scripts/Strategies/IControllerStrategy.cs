using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies;

public interface IControllerStrategy {
    #nullable enable
    public void Execute(InputEvent? @event, IControllable controllable);
}