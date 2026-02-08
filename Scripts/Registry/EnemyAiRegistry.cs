namespace CosmicDoom.Scripts.Registry;

using Godot;
using System.Collections.Generic;
using Entities;
using Interfaces;
using Strategies;
using Strategies.EnemyAI;

public partial class EnemyAiRegistry : Node, IRegistry<EnemyType, IEnemyAiStrategy> {
    private readonly Dictionary<EnemyType, IEnemyAiStrategy> _enemyAiRegistry = new() {
        [EnemyType.Destroyer] = new DestroyerStrategy()
    };
        
    public static EnemyAiRegistry INSTANCE { get; private set; }
        
    public IEnemyAiStrategy Get(EnemyType enemyType) {
        return _enemyAiRegistry.GetValueOrDefault(enemyType);
    }

    public IEnumerable<EnemyType> GetKeys() {
        return _enemyAiRegistry.Keys;
    }
    
    public override void _Ready() {
        INSTANCE = this;
    }
}
