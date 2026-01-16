using Godot;

namespace CosmicDoom.Scripts.Items;
using Strategies;

public enum WeaponType {
    Knife,
    MachineGun,
    PlasmaGun,
    RocketLauncher,
    Shotgun,
    Solution,
    None
}

public record RWeapon(
    WeaponType TYPE,
    int DAMAGE,
    CompressedTexture2D TEXTURE,
    IWeaponStrategy STRATEGY
);