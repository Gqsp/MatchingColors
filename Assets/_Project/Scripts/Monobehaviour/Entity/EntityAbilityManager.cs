using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityAbilityManager : EntityComponent
{
    [SerializeField] MM_Gravity _gravity;
    [SerializeField] List<EntityMovementModule> _startAbilities;

    readonly Dictionary<Type, EntityMovementModule> _fastQuery = new();
    readonly List<EntityMovementModule> _abilities = new();
    readonly List<EntityMovementModule> _skipModule = new();
    
    bool _isPaused;
    bool _skipAll;

    EntityMovementCore _movementCore;

    private void Awake()
    {
        _movementCore = core.Query<EntityMovementCore>();

        _startAbilities.AddRange(GetComponentsInChildren<EntityMovementModule>().ToList());

        foreach (var ability in _startAbilities) AddAbility(ability);
        
        if (_gravity != null)
        {
            var instance = AddAbility(_gravity);
            _movementCore.SetGravityModule(instance as MM_Gravity);
        }
    }

    public void UpdateAll()
    {
        if (_isPaused) return;
        
        _skipModule.Clear();
        _skipAll = false;
        
        foreach (var ability in _abilities)
        {
            if (_skipAll) break;
            if (_skipModule.Contains(ability)) continue;
            ability.UpdateModule();
        }
    }

    public void AddSkip(EntityMovementModule module)
    {
        _skipModule.Add(module);
    }

    public void SkipAll()
    {
        _skipAll = true;
    }

    public EntityMovementModule AddAbility(EntityMovementModule ability)
    {
        var duplicate = _abilities.Find(a => a.id == ability.id);
        if (duplicate != null) return null;

        var instantiatedAbility = Instantiate(ability, transform);
        InsertOrdered(instantiatedAbility);
        instantiatedAbility.Setup(_movementCore, this);

        var resolvers = instantiatedAbility.GetUsedStates();
        foreach (var resolver in resolvers)
        {
            if (resolver == null) Debug.LogError(instantiatedAbility.id + " sending null resolver on " + name);
            _movementCore.AddResolver(resolver);
        }

        return instantiatedAbility;
    }

    public void RemoveAbility(EntityMovementModule ability)
    {
        var ownAbility = _abilities.Find(a => a.id == ability.id);
        if (ownAbility == null) return;

        _abilities.Remove(ability);

        var resolvers = ability.GetUsedStates();
        foreach (var resolver in resolvers) _movementCore.RemoveResolver(resolver);

        Destroy(ownAbility);
    }

    private void InsertOrdered(EntityMovementModule module)
    {
        IComparer<EntityMovementModule> c = MovementModuleComparer.Create<EntityMovementModule>((x, y) => x.priority - y.priority);
        int index = _abilities.BinarySearch(module, c);
        if (index < 0) _abilities.Add(module);
        else _abilities.Insert(index, module);
    }

    public T QueryModule<T>() where T : EntityMovementModule
    {
        if (_fastQuery.TryGetValue(typeof(T), out var t))
        {
            return t as T;
        }

        var system = _abilities.Find(system => system is T);
        _fastQuery.Add(typeof(T), system);
        return system as T;
    }
    
    public void PauseAbilities(bool pause)
    {
        _isPaused = pause;
    }
}

static class MovementModuleComparer
{
    public static IComparer<T> Create<T>(Func<T, T, int> comparer)
    {
        if (comparer == null) { throw new ArgumentNullException(nameof(comparer)); }
        return new TheComparer<T>(comparer);
    }
    private class TheComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> c;
        public TheComparer(Func<T, T, int> c) { this.c = c; }
        int IComparer<T>.Compare(T x, T y) { return c(x, y); }
    }
}
