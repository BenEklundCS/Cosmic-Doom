using Godot;

public partial class Lamp : Node3D {
    [Export(PropertyHint.Range, "0.0,2.0,0.05")] public float IntensityMultiplier = 1.0f;

    public override void _Ready() {
        var light = GetNode<OmniLight3D>("OmniLight3D");
        light.LightEnergy *= IntensityMultiplier;
    }
}
