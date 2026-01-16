using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Strategies.Weapon;
using Godot;

namespace CosmicDoom.Scripts.Registry;

using static GD;

public partial class WeaponRegistry : Node, IRegistry<WeaponType, RWeapon> {
    private readonly Dictionary<WeaponType, RWeapon> _weaponRegistry = new() {
        [WeaponType.Knife] = new RWeapon(
            WeaponType.Knife,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_knife.png"),
            new WeaponKnifeStrategy()
        ),
        [WeaponType.MachineGun] = new RWeapon(
            WeaponType.MachineGun,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_machinegun.png"),
            new WeaponMachineGunStrategy()
        ),
        [WeaponType.PlasmaGun] = new RWeapon(
            WeaponType.PlasmaGun,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_plasmagun.png"),
            new WeaponPlasmaGunStrategy()
        ),
        [WeaponType.RocketLauncher] = new RWeapon(
            WeaponType.RocketLauncher,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_rocketlauncher.png"),
            new WeaponRocketLauncherStrategy()
        ),
        [WeaponType.Shotgun] = new RWeapon(
            WeaponType.Shotgun,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_shotgun.png"),
            new WeaponShotgunStrategy()
        ),
        [WeaponType.Solution] = new RWeapon(
            WeaponType.Solution,
            10,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_solution.png"),
            new WeaponSolutionStrategy()
        ),
        [WeaponType.None] = new RWeapon(
            WeaponType.None,
            10,
            null,
            new WeaponDefaultStrategy()
        )
    };

    public static WeaponRegistry INSTANCE { get; private set; }

    public RWeapon Get(WeaponType weaponType) {
        return _weaponRegistry.GetValueOrDefault(weaponType);
    }

    public IEnumerable<WeaponType> GetKeys() {
        return _weaponRegistry.Keys;
    }

    public override void _Ready() {
        INSTANCE = this;
    }
}