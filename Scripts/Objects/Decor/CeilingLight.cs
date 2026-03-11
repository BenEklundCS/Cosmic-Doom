using Godot;
using static Godot.GD;
using System;
using CosmicDoom.Scripts;

public partial class CeilingLight : Node3D {
    [Export] public bool FlickerEnabled;
    [Export] public bool InstabilityEnabled;
    [Export(PropertyHint.Range, "0.0,2.0,0.05")] public float IntensityMultiplier = 1.0f;
    [Export] public float LightEnergy = 3.0f;
    [Export] public Vector2 OnDurationRange = new (3.0f, 15.0f);
    [Export] public Vector2 OffDurationRange = new (1.0f, 3.0f);
    [Export] public Vector2 InstabilityRange = new (0.8f, 1.0f);
    private OmniLight3D _light;
    private Timer _flickerTimer;
    private bool _on = true;

    public override void _Ready() {
        _light = GetNode<OmniLight3D>("OmniLight3D");
        _light.LightEnergy = LightEnergy * IntensityMultiplier;
        _flickerTimer = GetNode<Timer>("FlickerTimer");
        _flickerTimer.Timeout += OnFlickerTimerTimeout;
    }

    private void OnFlickerTimerTimeout() {
        if (!FlickerEnabled) return;
        
        _on = !_on;

        var energy = LightEnergy * IntensityMultiplier;
        if (InstabilityEnabled) {
            energy *= Utils.INSTANCE.NextFloat(InstabilityRange);
        }
        
        _flickerTimer.SetWaitTime(Utils.INSTANCE.NextFloat(_on ? OnDurationRange : OffDurationRange));
        _light.LightEnergy = _on ? energy : 0.0f;
        _flickerTimer.Start();
    }
}
