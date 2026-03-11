using Godot;
using System;

public partial class Observer : Node {
    private readonly Func<bool> _condition;
    private readonly Action _action;

    public Observer(Func<bool> condition, Action action) {
        _condition = condition;
        _action = action;
    }

    public override void _Process(double delta) {
        if (!_condition()) return;
        _action();
        QueueFree();
    }
}
