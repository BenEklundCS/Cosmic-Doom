using CosmicDoom.Scripts.Interfaces;
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
    PickupType AMMO_TYPE,
    int DAMAGE,
    bool RELOAD_ENABLED,
    int AMMO,
    int MAX_AMMO,
    float COOLDOWN,
    AtlasTexture TEXTURE,
    AtlasTexture ON_USE_TEXTURE,
    AtlasTexture ICON,
    AudioStreamWav[] ON_USE_AUDIO_STREAMS,
    AudioStreamWav ON_EQUIP_STREAM,
    IWeaponStrategy STRATEGY,
    bool IS_MELEE = false,
    Vector3? SHOT_OFFSET = null
) {
    // Default offset: down and forward from camera to gun barrel
    public Vector3 SHOT_OFFSET_ => SHOT_OFFSET ?? new Vector3(0, -0.3f, -0.5f);
}