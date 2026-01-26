using System.Threading;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Interfaces;
using Items;
using Context;

public partial class Weapon : Node3D, IWeapon {
    private AudioStreamPlayer3D _audio;
    private TextureRect _weaponRect;
    private TextureRect _onUseRect;
    private RWeapon _rWeapon;
    private Timer _cooldownTimer;
    private Timer _onUseRectVisibilityTimer;
    public override void _Ready() {
        _audio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        _weaponRect = GetNode<TextureRect>("Control/WeaponTexture");
        _onUseRect = GetNode<TextureRect>("Control/OnUseTexture");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _onUseRectVisibilityTimer = GetNode<Timer>("OnUseRectVisibilityTimer");
        _onUseRectVisibilityTimer.Timeout += () => { _onUseRect.Visible = false; };
    }

    public void Equip(RWeapon rWeapon) {
        _rWeapon = rWeapon;
        _weaponRect.Texture = rWeapon.TEXTURE;
        _onUseRect.Texture = rWeapon.ON_USE_TEXTURE;

        var randomizer = new AudioStreamRandomizer();
        for (int i = 0; i < _rWeapon.AUDIO_STREAMS.Length; i++) {
            randomizer.AddStream(i, _rWeapon.AUDIO_STREAMS[i]);
        }
        _audio.Stream = randomizer;
        
        _cooldownTimer.Stop();
        _cooldownTimer.SetWaitTime(rWeapon.COOLDOWN);
    }

    public void Use(RAttackContext context) {
        if (_cooldownTimer.IsStopped()) {
            _rWeapon.STRATEGY.Execute(context);
            _audio.Play();
            _cooldownTimer.Start();
            _onUseRectVisibilityTimer.Start();
            _onUseRect.Visible = true;
        }
    }
}