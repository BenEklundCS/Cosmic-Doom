using System;
using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Entities;

using static GD;

public abstract partial class Character : CharacterBody3D, IHittable {
    [Signal]
    public delegate void OnDeathEventHandler();
    
    [Export] public float JumpSpeed = 5.0f;
    [Export] public float Speed = 5.0f;
    [Export] public int MAX_HEALTH { get; private set; } = 100;
    [Export] public int MAX_ARMOR { get; private set; } = 50;
    [Export] public int HEALTH { get; protected set; }
    [Export] public int ARMOR { get; protected set; }
    [Export] public int DAMAGE { get; private set; } = 10;
    
    protected CollisionShape3D CollisionShape;
    protected Node3D Head;
    protected RayCast3D Ray;
    
    public virtual void Hit(int damage, Node3D attacker = null)
    {
        if (ARMOR > 0) {
            var armorRatio = (float)ARMOR / MAX_ARMOR; // Normalize armor to 0–1
            var reductionPercent = armorRatio * 0.5f; // Scale that 0–1 range to 0–0.5 (max 50% damage reduction at full armor)
            var reducedDamage = (int)(damage * (1f - reductionPercent)); // Apply reduction: 100% - reduction% of incoming damage
            var damageToArmor = Math.Min(ARMOR, reducedDamage); // Armor can only absorb up to its remaining value

            ARMOR -= damageToArmor; // Subtract absorbed damage from armor pool
            damage = reducedDamage - damageToArmor; // Set damage to remaining damage after hit
        }

        HEALTH -= damage; // Apply the remaining damage directly to health

        if (!(HEALTH <= 0)) return;
        // dead

        Print($"{Name} died.");
        EmitSignalOnDeath();
    }

    public override void _Ready() {
        Head = GetNode<Node3D>("Head");
        Ray = GetNode<RayCast3D>("Head/RayCast3D");
        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        HEALTH = MAX_HEALTH;
        base._Ready();
    }
}