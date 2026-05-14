# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

**Cosmic Doom** — a first-person shooter built in C# with Godot 4.4 (.NET 8).

## Build & Run

```bash
dotnet build          # Build the C# assembly
```

Run the game by pressing `F5` in the Godot 4.4 (.NET) editor. There is no test suite.

## Architecture

### Controller / IControllable (the key abstraction)

Both `Player` and `Enemy` implement `IControllable`. They don't decide *when* to act — they just execute actions. Input is separated from entities:

- `Controller.cs` — reads Godot `InputEvent` and calls `IControllable` methods directly (Player only)
- `AiController.cs` — behavior logic calls imperative methods which forward to `IEnemyControllable` (Enemy only)

**IEnemyControllable vs IControllable:** `Enemy` implements `IEnemyControllable` (extends `IControllable`) to add enemy-specific methods like `MoveTo()`, `FaceDirection()`, and health/state queries (`CanAttack()`, `HAS_RECOGNIZED_PLAYER`, etc.). `Player` implements `IControllable` directly. Both ultimately execute the same core actions (Move, Look, Attack) through the interface.

### Entity Hierarchy

```
CharacterBody3D -> Character (abstract: health, physics, OnDeath signal)
                    +-- Player  : Character, IControllable
                    +-- Enemy   : Character, IEnemyControllable
```

### Weapon System

`RWeapon` is an immutable C# record (data only). `WeaponRegistry` is a Godot Autoload singleton (`INSTANCE` pattern) that maps `WeaponType -> RWeapon`. Firing is delegated to an `IWeaponStrategy`:

- `HitscanStrategy` — raycast, instant hit; supports `shotCount` + `spreadDegrees` for shotguns
- `ProjectileStrategy` — spawns a projectile node (PlasmaBall, Rocket, Laser)
- `MeleeStrategy` — short range cone check

### Enemy AI

`UtilityAiStrategy` scores all candidate `IAction`s each tick and executes the highest scorer. Background actions (`IBackgroundAction`) always run (e.g. `FacePlayer`). Each enemy type has its own action set defined in `Scripts/Strategies/EnemyAI/Actions/<EnemyType>/`.

**Scoring system:**
- `Attack` action: Score = `1.0f` when `CanAttack()` (line of sight + recognized + weapon ready), else `0.0f`
- Move actions: Typically score `0.7f` (constant) or `0.9f` (aggressive charge)
- Panic actions: Score = `1.0f` when HP < threshold, else `0.0f`
- `IBackgroundAction` (e.g. `FacePlayer`): No score, always runs

Enemy recognition flow: raycast sees Player -> `ReactionTimer` fires -> `HAS_RECOGNIZED_PLAYER = true` -> `RememberTimer` resets it when sight is lost.

**Navigation:** `AiUtils` provides two helpers used by move actions:
- `GetMovePositionWherePlayerVisible(Enemy)` — Queries all nodes in group `"points"` (Point.cs nodes placed in scenes), raycasts from each toward player, returns nearest point with LOS to player
- `GetMovePositionWhereHidden(Enemy)` — Same query, returns farthest point without LOS (for retreat/panic)

Place Point nodes throughout levels to guide AI pathfinding and enable flanking behavior.

### Registries (Autoload Singletons)

All content data lives in registries accessed via `INSTANCE`:
- `WeaponRegistry` — `WeaponType -> RWeapon`
- `EnemyRegistry` — `EnemyType -> REnemy`
- `PickupRegistry` — `PickupType -> RPickup`

Records (`RWeapon`, `REnemy`, `RPickup`) in `Scripts/Items/` are immutable and hold textures, sounds, damage values, and strategy references.

**Six Autoload singletons:**
- `WeaponRegistry`, `EnemyRegistry`, `PickupRegistry` — content registries
- `EffectProvider` — spawns VFX (explosions, hit effects)
- `Utils` — utility helpers (LoadTrimmed image loading)
- `CyclopsAutoload` — level editor support

**Current enemy types (7 total):**
- `Destroyer` — ranged, retreats when damaged, full audio
- `Turret` — stationary ranged
- `Spider` — charges, has jump-away panic action
- `Ender`, `PlasmaBot` — standard ranged
- `Exploder` — suicide bomber (stub)
- `Warrior` — melee charger (todo: aggressive charge)

### Level Scripting

Level scripts (e.g. `LevelOne.cs`) extend `NavigationRegion3D`. The `LevelTriggers.WhenGroupDead(group, action)` extension method wires callbacks to enemy group deaths — used to spawn waves and open `TriggerableDoor`s.

Enemies are assigned to named groups via `[Export] StringName CustomGroupName` in the scene or set at spawn time.

### Projectiles

`Projectile.cs` is the base class. `PlasmaBall`, `Rocket`, and `Laser` extend it. `EffectProvider` handles VFX spawning (explosion effects, hit decals).

## Key Paths

| Path | Purpose |
|------|---------|
| `Scripts/Entities/` | Player, Enemy, Character base |
| `Scripts/Components/` | Controller, AiController, GravityComponent, FlashRed, MagazineFeed |
| `Scripts/Registry/` | WeaponRegistry, EnemyRegistry, PickupRegistry |
| `Scripts/Items/` | RWeapon, REnemy, RPickup records |
| `Scripts/Strategies/Weapon/` | HitscanStrategy, ProjectileStrategy, MeleeStrategy |
| `Scripts/Strategies/EnemyAI/` | UtilityAiStrategy, IAction, per-enemy action sets |
| `Scripts/Objects/Projectiles/` | Projectile base, PlasmaBall, Rocket, Laser |
| `Scripts/Levels/` | LevelOne, LevelTriggers extension |
| `Scenes/` | Godot scene files mirroring Scripts/ structure |