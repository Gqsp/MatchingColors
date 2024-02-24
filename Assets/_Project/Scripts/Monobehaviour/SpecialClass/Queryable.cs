using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Queryable<T> : MonoBehaviour where T : MonoBehaviour
{
    List<T> _components = new();
    Dictionary<Type, T> _fastQuery = new();

    private void Awake()
    {
        _components.AddRange(GetComponentsInChildren<T>().ToList());
    }

    public T2 QuerySystem<T2>() where T2 : T
    {
        if (_fastQuery.TryGetValue(typeof(T), out var t))
        {
            return t as T2;
        }

        var system = _components.Find(system => system is T2);
        _fastQuery.Add(typeof(T2), system);
        return system as T2;
    }
}
