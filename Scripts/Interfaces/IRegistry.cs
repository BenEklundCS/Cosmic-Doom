using System.Linq;

namespace CosmicDoom.Scripts.Interfaces;

using Godot;

public interface IRegistry<TRegistryKey, TRegistryValue> {
    public TRegistryValue Get(TRegistryKey key);
}