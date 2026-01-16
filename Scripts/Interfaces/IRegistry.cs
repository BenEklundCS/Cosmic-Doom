using System.Collections.Generic;
namespace CosmicDoom.Scripts.Interfaces;

public interface IRegistry<TRegistryKey, TRegistryValue> {
    public TRegistryValue Get(TRegistryKey key);
    public IEnumerable<TRegistryKey> GetKeys();
}