using System;
using CosmicDoom.Scripts.Interfaces;
using Godot;
using static Godot.GD;

public partial class HittableBox : StaticBody3D, IHittable {
    [Signal]
    public delegate void OnDeathEventHandler();

    private MeshInstance3D _mesh;
    private ShaderMaterial _shaderMaterial;
    [Export] public int HITS_TO_DESTROY { get; private set; } = 3;

    public void Hit(int damage) {
        // max damage is 1 as HittableBox has a "hits to destroy" not health
        var hits = Math.Clamp(damage, 0, 1);
        HITS_TO_DESTROY -= hits;
        _shaderMaterial.SetShaderParameter("redshift_amount", 1.0f);
        if (!(HITS_TO_DESTROY <= 0)) return;

        Print($"{Name} died.");
        EmitSignalOnDeath();
        QueueFree();
    }


    public override void _Ready() {
        _mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        _shaderMaterial = _mesh.GetActiveMaterial(0).Duplicate() as ShaderMaterial;
        _mesh.SetSurfaceOverrideMaterial(0, _shaderMaterial); // Apply the unique copy
        base._Ready();
    }
}