using System.Linq;

namespace CosmicDoom.Scripts.Entities;

using Godot;
using static Godot.GD;
using Context;
using Interfaces;
using Items;
using Components;
using Objects;
using Registry;

public enum EnemyState { Idle, Walking, Attacking, Dying }

public partial class Enemy : Character, IEnemyControllable {
    [Signal] public delegate void TargetReachedEventHandler();
    [Export] public bool Enabled = true;
    [Export] public EnemyType Type;
    [Export] public StringName CustomGroupName;
    [Export] public float MoveRange = 500.0f;
    [Export] public Vector2 MoveThinkingTimeRange = new (1.0f, 5.0f);
    [Export] public float AttackDuration = 0.08f;
    [Export] public float ReactionTime = 1.0f;
    [Export] public float RememberTime = 3.0f;
    [Export] public float BobAmplitude = 0.06f;
    [Export] public float BobFrequency = 10.0f;
    [Export] public int MagazineSize = 3;
    [Export] public int MaxAmmo = 300;

    private Pickup _pickupFactory = new();
    private REnemy _rEnemy;
    private RWeapon _weaponData;
    private Weapon _weapon;
    private NavigationAgent3D _navigationAgent;
    private AnimatedSprite3D _animatedSprite;
    private FlashRed _flashRed;
    private Timer _attackTimer;
    private Timer _reactionTimer;
    private Timer _rememberTimer;
    private float _bobTime = 0.0f;
    private float _spriteBaseY;
    private EnemyState _state = EnemyState.Idle;

    public float DISTANCE_TO_PLAYER => NEAREST_PLAYER != null 
        ? GlobalPosition.DistanceTo(NEAREST_PLAYER.GlobalPosition) 
        : float.MaxValue;
    public Player NEAREST_PLAYER { get; private set; }
    public bool IS_MOVING => _state == EnemyState.Walking;
    public bool HAS_RECOGNIZED_PLAYER { get; private set; } = false;
    public float HEALTH_PERCENT => (float)HEALTH / MAX_HEALTH;

    public override void _Ready() {
        _rEnemy = EnemyRegistry.INSTANCE.Get(Type);
        var baseWeapon = WeaponRegistry.INSTANCE.Get(_rEnemy.WEAPON_TYPE);
        _weaponData = baseWeapon with { AMMO = MagazineSize, MAX_AMMO = MaxAmmo };
        _weapon = GetNode<Weapon>("Weapon");
        _weapon.Equip(_weaponData);
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.TargetReached += OnTargetReached;
        _animatedSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
        _animatedSprite.SpriteFrames = _rEnemy.SPRITE_FRAMES;
        _animatedSprite.Play("idle");
        _spriteBaseY = _animatedSprite.Position.Y;
        _flashRed = GetNode<FlashRed>("FlashRed");
        _attackTimer = GetNode<Timer>("AttackTimer");
        _attackTimer.SetWaitTime(AttackDuration);
        _attackTimer.Timeout += OnAttackTimerTimeout;
        _reactionTimer = GetNode<Timer>("ReactionTimer");
        _reactionTimer.SetWaitTime(ReactionTime);
        _reactionTimer.Timeout += OnReactionTimerTimeout;
        _rememberTimer = GetNode<Timer>("RememberTimer");
        _rememberTimer.SetWaitTime(RememberTime);
        _rememberTimer.Timeout += OnRememberTimerTimeout;

        OnDeath += OnSelfDeath;
        _animatedSprite.AnimationFinished += OnAnimationFinished;
        
        AddToGroup("enemies");
        if (CustomGroupName != null) AddToGroup(CustomGroupName);

        base._Ready();
    }

    public override void _Process(double delta) {
        NEAREST_PLAYER = GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => GlobalPosition.DistanceTo(player.GlobalPosition));
        
        // if we can currently see, react (and stop the remember timer so the bot won't "forget" while he's reacting)
        var canCurrentlySee = Ray.GetCollider() is Player;
        if (canCurrentlySee && _reactionTimer.IsStopped()) {
            _reactionTimer.Start();
            _rememberTimer.Stop();
        }
        else {
            _rememberTimer.Start();
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (_navigationAgent == null) return;
        if (_navigationAgent.IsNavigationFinished()) {
            Velocity = new Vector3(0, Velocity.Y, 0);
            _bobTime = 0.0f;
            var pos = _animatedSprite.Position;
            pos.Y = _spriteBaseY;
            _animatedSprite.Position = pos;
            return;
        }

        var nextPosition = _navigationAgent.GetNextPathPosition();
        var direction = (nextPosition - GlobalPosition).Normalized();
        Velocity = new Vector3(direction.X * Speed, Velocity.Y, direction.Z * Speed);

        HandleBob(delta);
        
        base._PhysicsProcess(delta);
    }

    public void MoveTo(Vector3 position) {
        _navigationAgent.TargetPosition = position;
        SetState(EnemyState.Walking);
    }

    public void FaceTarget(Vector3 position) {
        LookAt(position);
    }

    public void Attack() {
        RAttackContext context = new(
            -Head.GlobalBasis.Z,
            Ray,
            _weaponData,
            this
        );

        if (_weapon.Use(context) && _state != EnemyState.Attacking) {
            SetState(EnemyState.Attacking);
            _attackTimer.Start();
        }
    }

    public bool CanAttack() {
        return CanSeePlayer() && HAS_RECOGNIZED_PLAYER && !_weapon.OnCooldown();
    }

    public override void Hit(int damage) {
        if (_state == EnemyState.Dying) return;
        _flashRed.Trigger();
        base.Hit(damage);
    }
    
    private void OnAttackTimerTimeout() {
        SetState(_navigationAgent.IsNavigationFinished() ? EnemyState.Idle : EnemyState.Walking);
    }

    private void OnReactionTimerTimeout() {
        // neuron has fired, if we still see them set true
        if (CanSeePlayer()) {
            HAS_RECOGNIZED_PLAYER = true;
        }
    }

    private void OnRememberTimerTimeout() {
        HAS_RECOGNIZED_PLAYER = false; // no longer remembers player
    }

    private void OnTargetReached() {
        if (_state != EnemyState.Attacking)
            SetState(EnemyState.Idle);
        EmitSignalTargetReached();
    }

    private void SetState(EnemyState newState) {
        _state = newState;
        _animatedSprite.Play(newState switch {
            EnemyState.Walking => "walk",
            EnemyState.Attacking => "attack",
            EnemyState.Dying => "dying",
            _ => "idle"
        });
    }

    private void HandleBob(double delta) {
        if (_state == EnemyState.Walking) {
            _bobTime += (float)delta;
            var bobOffset = Mathf.Sin(_bobTime * BobFrequency) * BobAmplitude;
            var spritePos = _animatedSprite.Position;
            spritePos.Y = _spriteBaseY + bobOffset;
            _animatedSprite.Position = spritePos;
        } else {
            _bobTime = 0.0f;
            var pos = _animatedSprite.Position;
            pos.Y = _spriteBaseY;
            _animatedSprite.Position = pos;
        }
    }

    private void OnSelfDeath() {
        if (_state == EnemyState.Dying) return;
        SetState(EnemyState.Dying);
        DropAmmo();
    }

    private void DropAmmo() {
        foreach (var type in _rEnemy.PICKUPS) {
            var shouldDrop = RandRange(0, 4) == 1;
            if (!shouldDrop) continue;
            var pickup = (Pickup)_pickupFactory.Spawn();
            pickup.Type = type;
            GetTree().Root.AddChild(pickup);
            var offset = new Vector3(
                RandRange(-8, 8),
                0,
                RandRange(-8, 8)
            );
            pickup.GlobalPosition = GlobalPosition + offset;
        }
    }
    
    private void OnAnimationFinished() {
        if (_state == EnemyState.Dying) {
            QueueFree();
        }
    }
    
    private bool CanSeePlayer() {
        return Ray.GetCollider() is Player;
    }
}
