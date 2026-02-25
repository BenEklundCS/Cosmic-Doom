using Godot;

namespace CosmicDoom.Scripts.Items;

public enum PickupCategory {
    Default, // Anything the player doesn't specifically need to route
    Ammo, // Bullets/Plasma/Rockets
    Life, // Health/Shield
}

public enum PickupType {
    None,
    // Ammo
    Bullets,
    Plasma,
    Rockets,
    // Life
    Health,
    Armor,
    // Items
    Keycard
}

public record RPickup(
    PickupType TYPE,
    PickupCategory CATEGORY,
    Texture2D TEXTURE
);