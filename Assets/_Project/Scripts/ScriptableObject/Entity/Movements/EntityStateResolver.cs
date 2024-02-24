using UnityEngine;

public abstract class EntityStateResolver : ScriptableObject
{
    [SerializeField] protected string _name;
    public string State => _name;
    public abstract bool ResolveState(EntityMovementCore entity);

}
