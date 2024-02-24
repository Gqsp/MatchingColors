using System.Collections.Generic;
using UnityEngine;

public class EntityMovementCore : EntityComponent
{
    [SerializeField] AnimationHandler _animatorHandler;
    EntityCollisionSolver _collisionSolver;
    EntityAbilityManager _abilityManager;
    InputHandler _inputHandler;
    Rigidbody2D _rb;

    MM_Gravity _gravity;
    public EntityParticleHandler ParticleHandler { get; private set; }

    readonly Dictionary<EntityStateResolver, int> _stateResolvers = new();
    readonly Dictionary<string, EntityStateResolverResponse> _frameState = new();
    
    public EntityActionData ActionData => _inputHandler.ActionData;
    public CollisionData FrameCollisionData => _collisionSolver.CollisionData;
    public AnimationHandler AnimatorHandler => _animatorHandler;
    public Vector2 Velocity { get; set; }
    public Vector2 LastFrameVelocity { get; private set; }
    public float GravityScale { get; set; } = 1;

    private void Awake()
    {
        _inputHandler = core.Query<InputHandler>();
        _collisionSolver = core.Query<EntityCollisionSolver>();
        ParticleHandler = core.Query<EntityParticleHandler>();
        _abilityManager = core.Query<EntityAbilityManager>();

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateStates();

        _abilityManager.UpdateAll();
        
        _rb.velocity = Velocity;
        LastFrameVelocity = Velocity;
        Velocity = Vector2.zero;
    }

    public float GetGravity()
    {
        return _gravity ? _gravity.GetGravity() : 0.0f;
    }

    public void AddResolver(EntityStateResolver newResolver)
    {
        if (_stateResolvers.ContainsKey(newResolver))
            _stateResolvers[newResolver]++;
        else _stateResolvers.Add(newResolver, 1);
    }

    public void RemoveResolver(EntityStateResolver removedResolver)
    {
        if (_stateResolvers.ContainsKey(removedResolver))
        {
            _stateResolvers[removedResolver]--;
            if (_stateResolvers[removedResolver] <= 0) _stateResolvers.Remove(removedResolver);
        }
    }

    public bool TryGetState(EntityStateResolver resolver, out EntityStateResolverResponse state)
    {
        return _frameState.TryGetValue(resolver.State, out state);
    }

    void UpdateStates()
    {
        foreach (EntityStateResolver resolver in _stateResolvers.Keys)
        {
            SaveState(resolver.State, resolver.ResolveState(this));
        }
    }

    void SaveState(string stateName, bool status)
    {
        if (_frameState.TryGetValue(stateName, out var savedState))
        {
            savedState.UpdateValue(status);
        } else
        {
            var newSavedState = new EntityStateResolverResponse
            {
                validPreviousFrame = false,
                validThisFrame = status
            };
            _frameState.Add(stateName, newSavedState);
        }
    }

    public void SetGravityModule(MM_Gravity gravityModule)
    {
        _gravity = gravityModule;
    }
}
