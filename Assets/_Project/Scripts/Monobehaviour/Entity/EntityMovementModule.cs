using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovementModule : MonoBehaviour
{
    public string id;
    public int priority;

    protected EntityMovementCore _core;
    protected EntityAbilityManager _abilities;

    public void Setup(EntityMovementCore core, EntityAbilityManager abilities)
    {
        _core = core;
        _abilities = abilities;

        name = id;
    }

    public abstract List<EntityStateResolver> GetUsedStates();
    public virtual void UpdateModule() { }
}

public abstract class EntityMovementModule<T> : EntityMovementModule where T : ModuleProfile
{
    [SerializeField] protected ModuleProfileManager<T> _profiles;
}
