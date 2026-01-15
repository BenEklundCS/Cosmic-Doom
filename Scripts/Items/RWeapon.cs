using Godot;

namespace CosmicDoom.Scripts.Items;

public enum WeaponType {
    Knife,
    MachineGun,
    PlasmaGun,
    RocketLauncher,
    Shotgun,
    Solution,
    None,
}

public record RWeapon(
    WeaponType Type,
    CompressedTexture2D Texture
);