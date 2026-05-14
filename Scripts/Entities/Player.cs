namespace CosmicDoom.Scripts.Entities;

using System;
using System.Collections.Generic;
using Context;
using Interfaces;
using Items;
using Registry;
using Godot;
using Objects;
using UI;
using Components;


public partial class Player : Character, IControllable {
    private readonly WeaponType[] _defaultWeapons = new[] {
        WeaponType.None,
        WeaponType.Knife,
        WeaponType.PlasmaGun,
        WeaponType.Shotgun,
        WeaponType.RocketLauncher
    };
    private readonly List<RWeapon> _weaponWheel = new();
    
    private int _weaponIndex;
    private readonly float _maxPitch = Mathf.DegToRad(85f);
    private CameraShake _camera;
    private Weapon _weapon;
    private HealthBar _healthBar;
    private HealthBar _armorBar;
    private Area3D _pickupArea;
    private AudioStreamPlayer3D _hurtSound;
    private ColorRect _hurtOverlay;

    [Export] public float MouseSensitivity = 0.005f;

    public override void _Ready() {
        _weapon = GetNode<Weapon>("Weapon");
        _camera = GetNode<CameraShake>("Head/Camera3D");
        _healthBar = GetNode<HealthBar>("HealthBar");
        _armorBar = GetNode<HealthBar>("ArmorBar");
        _pickupArea = GetNode<Area3D>("PickupArea");
        _pickupArea.BodyEntered += OnBodyEnteredPickupArea;
        _hurtSound = GetNode<AudioStreamPlayer3D>("HurtSound");
        _hurtOverlay = GetNode<ColorRect>("HurtOverlay/ColorRect");

        ReadyWeapons();

        AddToGroup("players");

        OnDeath += OnSelfDeath;

        base._Ready();
    }

    public override void _Process(double delta) {
        _healthBar.SetHealth(HEALTH, MAX_HEALTH);
        _armorBar.SetHealth(ARMOR, MAX_ARMOR);
        base._Process(delta);
    }

    public void Move(Vector3 movement) {
        var direction = Transform.Basis * movement;
        
        Velocity = new Vector3(
            direction.X * Speed,
            Velocity.Y,
            direction.Z * Speed
        );
    }

    public void Jump() {
        if (IsOnFloor())
            Velocity = new Vector3(Velocity.X, JumpSpeed, Velocity.Z);
    }
    
    public void Look(Vector2 relative) {
        RotateY(-relative.X * MouseSensitivity);
        Head.RotateX(-relative.Y * MouseSensitivity);
        var clampedX = Math.Clamp(
            Head.Rotation.X,
            -_maxPitch,
            _maxPitch
        );
        Head.Rotation = new Vector3(clampedX, Head.Rotation.Y, Head.Rotation.Z);
    }
    
    public override void Hit(int damage, Node3D attacker = null) {
        _weapon.FlashIcon();

        // Play hurt sound
        if (_hurtSound != null) {
            _hurtSound.Play();
        }

        // Flash screen overlay red
        if (_hurtOverlay != null) {
            var color = _hurtOverlay.Color;
            color.A = 0.4f;
            _hurtOverlay.Color = color;

            var tween = _hurtOverlay.CreateTween();
            tween.SetTrans(Tween.TransitionType.Linear);
            tween.TweenProperty(_hurtOverlay, "color:a", 0f, 0.3);
        }

        // Camera shake on impact
        if (_camera != null) {
            _camera.Shake(0.2f, 0.3f);
        }

        base.Hit(damage / 2, attacker);
    }

    public void Attack() {
        var weapon = _weaponWheel[_weaponIndex];
        RAttackContext context = new(
            -Head.GlobalBasis.Z, 
            Ray, 
            weapon, 
            this
        );
        _weapon.Use(context);
    }

    public void NextWeapon() {
        _weaponIndex = Mathf.PosMod(_weaponIndex + 1, _weaponWheel.Count);
        EquipWeapon(_weaponIndex);
    }

    public void PrevWeapon() {
        _weaponIndex = Mathf.PosMod(_weaponIndex - 1, _weaponWheel.Count);
        EquipWeapon(_weaponIndex);
    }

    private void EquipWeapon(int weaponIndex) {
        var weapon = _weaponWheel[weaponIndex];
        _weapon.Equip(weapon);
    }
    
    private void ReadyWeapons() {
        foreach (var weaponType in _defaultWeapons) {
            var rWeapon = WeaponRegistry.INSTANCE.Get(weaponType);
            _weaponWheel.Add(rWeapon);
            _weapon.InitializeFeed(rWeapon);
        }
        EquipWeapon(_weaponIndex);
    }

    private void OnBodyEnteredPickupArea(Node3D body) {
        if (body is not Pickup pickup) return;

        switch (pickup.Category) {
            case PickupCategory.Ammo: {
                _weapon.PickupAmmo(pickup);
                break;
            }
            case PickupCategory.Life: {
                PickupLife(pickup);
                break;
            }
            default:
                break;
        }

        pickup.Consume();
    }

    private void PickupLife(Pickup pickup) {
        switch (pickup.Type) {
            case PickupType.Health: {
                HEALTH = MAX_HEALTH;
                break;
            }
            case PickupType.Armor: {
                ARMOR = MAX_ARMOR;
                break;
            }
            default:
                break;
        }
    }

    private void OnSelfDeath() {
        QueueFree();
    }
}