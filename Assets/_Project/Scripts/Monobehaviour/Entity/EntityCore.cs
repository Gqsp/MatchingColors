using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityCore : MonoBehaviour
{
    List<EntityComponent> _components = new();
    Dictionary<Type, EntityComponent> _fastQuery = new();

    private void Awake()
    {
        _components = GetComponentsInChildren<EntityComponent>().ToList();
        _components.ForEach(s => s.Setup(this));
    }

    public T Query<T>() where T : EntityComponent
    {
        if (_fastQuery.TryGetValue(typeof(T), out var t))
        {
            return t as T;
        }

        var system = _components.Find(system => system is T);
        _fastQuery.Add(typeof(T), system);
        return system as T;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
