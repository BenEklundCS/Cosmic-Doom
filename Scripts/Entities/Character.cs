using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts;

using Godot;
using static Godot.GD;
using System;

public abstract partial class Character : CharacterBody3D, IHittable {
    [Signal] public delegate void OnDeathEventHandler();
    [Export] public int HEALTH { get; private set; } = 100;
    [Export] public int DAMAGE { get; private set; } = 10;
    [Export] public float Speed = 5.0f;
    [Export] public float JumpSpeed = 5.0f;
    
    protected RayCast3D Ray;
    protected CollisionShape3D CollisionShape;
    protected Node3D Head;

    public override void _Ready() {
        Head = GetNode<Node3D>("Head");
        Ray = GetNode<RayCast3D>("Head/RayCast3D");
        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        base._Ready();
    }

    public void Hit(int damage) {
        HEALTH -= damage;
        if (!(HEALTH <= 0)) return;
        
        Print($"{Name} died.");
        EmitSignalOnDeath();
        QueueFree();
    }
}
