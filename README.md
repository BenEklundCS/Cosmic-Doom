# Cosmic Doom

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Godot](https://img.shields.io/badge/Godot_4.4-478CBF?style=for-the-badge&logo=godotengine&logoColor=white)](https://godotengine.org/)
[![.NET 8](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)

## Overview
**Cosmic Doom** is a first-person shooter built in **C#** using the **Godot Engine**, featuring custom weapon systems inspired by 1990s FPS games like *Doom* and *Wolfenstein*. The project is currently in active development.

---

## Key Features

- **Arsenal of Weapons:**
  Knife, Machine Gun, Shotgun, Plasma Gun, Rocket Launcher, and more — each with distinct firing strategies including hitscan, projectile, and melee.

- **Diverse Enemy Types:**
  Seven enemy types with unique behaviors — stationary turrets, melee-charging warriors, suicide bombers, rocket-jumping spiders, and more — powered by a utility-based AI system.

- **Utility AI:**
  Enemies evaluate and score actions each tick (attack, move, panic, retreat) to make dynamic combat decisions based on health, distance, and line of sight.

- **Data-Driven Design:**
  Weapons, enemies, and pickups are defined as immutable records in singleton registries, making it easy to add and configure new content.

- **Strategy Pattern:**
  Weapon firing and enemy AI behaviors are implemented as interchangeable strategies, keeping game logic modular and extensible.

- **Pickup & Loot System:**
  Ammo, health, armor, and keycards drop from enemies and are scattered throughout levels. Keycards unlock doors.

- **Triggerable Doors:**
  Doors that open via keycard pickups or automatically when all enemies in a group are eliminated.

- **Projectile System:**
  Physics-based projectiles (plasma balls, rockets, lasers) with inherited scene architecture, explosion VFX, and positional audio.

- **Armor & Damage Reduction:**
  Player armor absorbs incoming damage before health.

- **Destructible Objects:**
  Barrels and crates can be destroyed by both the player and enemies.

---

## Controls
- **WASD:** Move
- **Mouse:** Look
- **Left Click:** Attack
- **Scroll Wheel:** Cycle weapons
- **Space:** Jump

---

## Architecture

```
Scripts/
  Context/        Attack context passed to weapon strategies
  Components/     Reusable components (gravity, flash, AI controller, magazine feed)
  Entities/       Character, Player, Enemy
  Interfaces/     IHittable, IProjectile, IControllable, IRegistry, IWeapon
  Items/          Data records (RWeapon, REnemy, RPickup)
  Objects/        World objects (Weapon, Pickup, TriggerableDoor, Barrel)
    Projectiles/  Projectile base + PlasmaBall, Rocket, Laser
  Provider/       EffectProvider (VFX spawning)
  Registry/       WeaponRegistry, EnemyRegistry, PickupRegistry
  Strategies/
    Weapon/       HitscanStrategy, ProjectileStrategy, MeleeStrategy
    EnemyAI/      UtilityAiStrategy + per-enemy action sets
  UI/             HUD components
```

---

## Getting Started

### Prerequisites
- [**Godot 4.4**](https://godotengine.org/download/) (.NET / C# version)
- [**.NET 8 SDK**](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Recommended IDE
[**JetBrains Rider**](https://www.jetbrains.com/rider/) with the [Godot support plugin](https://plugins.jetbrains.com/plugin/13882-godot-support) is recommended for the best C# + Godot development experience.

### Installation

1. **Clone the repository**
   ```
   git clone https://github.com/BenEklundCS/Cosmic-Doom.git
   cd Cosmic-Doom
   ```

2. **Open in Godot**
   Launch Godot 4.4 (.NET version) and import the `project.godot` file.

3. **Build and run**
   Press `F5` in the Godot editor to run. You can also build the C# assembly independently via the .NET CLI (requires Godot to run):
   ```
   dotnet build
   ```
