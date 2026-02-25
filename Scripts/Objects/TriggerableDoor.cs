using Godot;
using System;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Objects;

public partial class TriggerableDoor : Node3D {
    [Export] public Pickup Keycard;
    [Export] public float MoveDistance = 2.1f;
    [Export] public float MoveDuration = 1.0f;
    
    private MeshInstance3D _mesh;
    private bool _isOpen;

    private Vector3 _meshStart;
    private Vector3 _meshEnd;

    public override void _Ready() {
        // allow null keycards, but if not null and misconfigured throw error
        if (Keycard != null) {
            if (Keycard.Type != PickupType.Keycard) {
                throw new Exception("Invalid door configuration. Please set a PickupType.Keycard as this Pickup");
            }
            Keycard.Consumed += OnKeycardConsumed;
        }
        
        _mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        _meshStart = _mesh.GlobalPosition;
        _meshEnd = new Vector3(_meshStart.X, _meshStart.Y + MoveDistance, _meshStart.Z);
    }

    public void SetDoorOpen(bool open) {
        _isOpen = open;

        var tween = CreateTween();
        var targetPosition = open ? _meshEnd : _meshStart;

        tween.TweenProperty(_mesh, "global_position", targetPosition, MoveDuration);
    }

    private void OnKeycardConsumed() {
        SetDoorOpen(true);
    }
}
