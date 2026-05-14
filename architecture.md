# Cosmic Doom Architecture

## Controller System

Player and AI converge at `IControllable`. The key insight: both paths end up calling the same interface methods, just driven by different sources (input events vs AI logic).

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│  ┌─────────────┐                                                   │
│  │ InputEvent  │                                                   │
│  └──────┬──────┘                                                   │
│         │                                                          │
│         ▼                                                          │
│  ┌─────────────┐         ┌──────────────┐                         │
│  │ Controller  │────────→│ IControllable │←───────┐                │
│  └─────────────┘         └──────────────┘        │                │
│    (polls Input,              ▲                   │                │
│     handles events)           │                   │                │
│                               │            ┌──────┴──────┐         │
│                               │            │ AIController │         │
│                               │            └──────┬──────┘         │
│                        ┌──────┴──────┐           │                │
│                        │   Player    │     ┌─────┴─────┐          │
│                        │   Enemy     │     │ Behavior  │          │
│                        └─────────────┘     │ Logic     │          │
│                        (implement the      └───────────┘          │
│                         interface)         (state machine,        │
│                                             behavior tree,        │
│                                             or simple scripts)    │
└─────────────────────────────────────────────────────────────────────┘
```

### Player Input Flow

Controller directly interprets input and calls IControllable methods:

```
InputEvent
    │
    ▼
Controller._Input()
    ├─→ MouseMotion      → _controlTarget.Look(relative)
    ├─→ "escape"         → Release mouse capture
    ├─→ "click"          → Capture mouse OR _controlTarget.Attack()
    ├─→ "jump"           → _controlTarget.Jump()
    ├─→ "wheel_up"       → _controlTarget.NextWeapon()
    └─→ "wheel_down"     → _controlTarget.PrevWeapon()

Controller._PhysicsProcess()
    └─→ Input.GetVector() → _controlTarget.Move(direction)
```

### AI Flow

AIController exposes imperative methods that behavior logic calls:

```
┌──────────────────┐     ┌──────────────┐     ┌──────────────┐
│  Behavior Logic  │────→│ AIController │────→│ IControllable │
│  (your AI code)  │     │              │     │              │
└──────────────────┘     └──────────────┘     └──────────────┘

Behavior calls:              Which calls:
  ai.SetMoveDirection()  →     _controlTarget.Move()
  ai.Attack()            →     _controlTarget.Attack()
  ai.Jump()              →     _controlTarget.Jump()
  ai.LookAt(position)    →     _controlTarget.Look() [TODO: compute delta]
```

## IControllable Interface

The shared contract for anything that can be controlled (Player, Enemy).

```csharp
interface IControllable {
    void Attack();
    void Look(Vector2 relative);
    void Move(Vector3 direction);
    void Jump();
    void NextWeapon();
    void PrevWeapon();
}
```

### Why This Works for Both Player and AI

| Method | Player (via Controller) | AI (via AIController) |
|--------|------------------------|----------------------|
| `Move()` | Polls WASD every physics frame | Behavior sets direction based on pathfinding |
| `Look()` | Mouse delta from InputEvent | Computed delta to face target position |
| `Attack()` | Mouse click when captured | Behavior decides when to attack |
| `Jump()` | Spacebar press | Behavior decides (e.g., obstacle avoidance) |

The entity (Player/Enemy) doesn't care *why* these methods are called - it just executes the action.

## Entity Hierarchy

```
CharacterBody3D
       │
       ▼
  ┌─────────┐
  │Character│  (abstract)
  │─────────│
  │ Health  │
  │ Speed   │
  │ Gravity │
  │ Hit()   │
  └────┬────┘
       │
   ┌───┴───┐
   ▼       ▼
┌──────┐ ┌─────┐
│Player│ │Enemy│
│──────│ │─────│
│:IControllable│ │:IControllable│
└──────┘ └─────┘
```

## Weapon System

Weapons use the Strategy pattern correctly - different weapons have genuinely different firing behaviors.

```
IControllable.Attack()
        │
        ▼
┌───────────────┐     ┌───────────────┐
│   RWeapon     │────→│IWeaponStrategy│
│ (data record) │     │  .Execute()   │
└───────────────┘     └───────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│HitscanStrategy│     │ProjectileStrategy│   │ MeleeStrategy │
│ (raycast,     │     │ (spawn node,  │     │ (short range, │
│  instant hit) │     │  return it)   │     │  cone check)  │
└───────────────┘     └───────────────┘     └───────────────┘
```

### Weapon Registry

```
WeaponRegistry (Autoload Singleton)
       │
       │ Get(WeaponType)
       ▼
   ┌────────┐
   │ RWeapon │ ──→ { Type, Texture, Damage, Strategy }
   └────────┘
```

## File Structure

```
Scripts/
├── Components/
│   ├── Controller.cs        # Player input → IControllable (direct calls)
│   └── AIController.cs      # AI behavior → IControllable
├── Entities/
│   ├── Character.cs         # Base class (health, physics)
│   ├── Player.cs            # : Character, IControllable
│   └── Enemy.cs             # : Character, IControllable
├── Interfaces/
│   ├── IControllable.cs     # The convergence point
│   ├── IHittable.cs         # Damage system
│   └── IRegistry.cs         # Generic registry pattern
├── Registry/
│   └── WeaponRegistry.cs    # Weapon data lookup
└── Strategies/
    ├── IWeaponStrategy.cs   # Weapon behavior contract
    └── Weapon/
        ├── HitscanStrategy.cs
        ├── ProjectileStrategy.cs
        ├── MeleeStrategy.cs
        └── Weapon*Strategy.cs  # Per-weapon implementations (legacy)
```

## Design Principles

1. **IControllable is the convergence point** - Player and AI both call these same methods
2. **Controller is simple** - Direct input → method calls, no abstraction layers
3. **AIController is imperative** - Behavior logic tells it what to do, it forwards to IControllable
4. **Weapon strategies are justified** - Genuinely different algorithms (hitscan vs projectile vs melee)
5. **Entities are dumb** - They execute actions, they don't decide when to act
