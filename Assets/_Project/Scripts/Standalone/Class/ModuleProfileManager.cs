using System;
using UnityEngine;

[Serializable]
public class ModuleProfileManager<T> where T : ModuleProfile
{
    [SerializeField] int _defaultIndex;
    [SerializeField] T[] _profiles;

    public T GetProfile()
    {
        if (_profiles.Length == 0) return null;
        return _profiles[_defaultIndex % _profiles.Length];
    }
    
    public T GetProfile(int index)
    {
        if (_profiles.Length == 0) return null;
        return _profiles[index % _profiles.Length];
    }
}
