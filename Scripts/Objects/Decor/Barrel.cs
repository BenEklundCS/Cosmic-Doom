namespace CosmicDoom.Scripts.Objects.Decor;

using Godot;
using Effects;
using Interfaces;
using Registry;

public partial class Barrel : StaticBody3D, IHittable {
    private AudioStreamPlayer3D _explosionSound;
    private Area3D _explosionBox;
    private bool _exploded; // prevent recursive explosion loops (e.g. barrel a hits b hits a etc)
    [Export] public int Damage = 100;
    
    public override void _Ready() {
        _explosionSound = GetNode<AudioStreamPlayer3D>("ExplodeSound");
        _explosionBox = GetNode<Area3D>("Area3D");
    }

    public void Hit(int damage) {
        Explode();
    }

    private void Explode() {
        if (_exploded) return;
        _exploded = true;
        
        EffectProvider.INSTANCE.SpawnEffectAt(EffectType.Explosion, GlobalPosition);
        
        var bodies = _explosionBox.GetOverlappingBodies();
        foreach (var body in bodies) {
            if (body is IHittable hittable) hittable.Hit(Damage);
        }
        
        _explosionSound.Reparent(GetTree().Root);
        _explosionSound.Play();
        _explosionSound.Finished += _explosionSound.QueueFree;

        QueueFree();
    }
}
