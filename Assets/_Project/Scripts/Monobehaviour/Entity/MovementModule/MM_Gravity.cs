using System.Collections.Generic;
using UnityEngine;

public class MM_Gravity : EntityMovementModule<GravityProfile>
{
    [SerializeField] EntityStateResolver AirbornResolver;
    [SerializeField] EntityStateResolver GroundedResolver;
    public override List<EntityStateResolver> GetUsedStates()
    {
        return new List<EntityStateResolver> { AirbornResolver, GroundedResolver };
    }

    float _gravityMultiplier;
    float _fallingTime;
    float _startMultiplier;
    float _frameGravity;

    private GravityProfile _data;

    private void Start()
    {
        _data = _profiles.GetProfile();
    }


    public override void UpdateModule()
    {
        if (!_core.TryGetState(AirbornResolver, out var airborn)) return;
        if (!_core.TryGetState(GroundedResolver, out var grounded)) return;
        if (grounded.validThisFrame) { _fallingTime = 0; _frameGravity = 0; return; }
        
        if (!airborn.validThisFrame)
        {
            _fallingTime = 0;
        }

        if (_core.LastFrameVelocity.y < 0)
        {
            _fallingTime += Time.fixedDeltaTime;
            _gravityMultiplier = Mathf.SmoothStep(_startMultiplier, _data.terminalVelocity, Mathf.Clamp01(_fallingTime / _data.timeToTerminalVelocity));
        }
        else if (_core.LastFrameVelocity.y > 0)
        {
            _gravityMultiplier = _data.upwardGravityMultiplier;
            _startMultiplier = _data.upwardGravityMultiplier;
        }
        else
        {
            _gravityMultiplier = 1;
            _startMultiplier = 1;
            _fallingTime = 0;
        }

        _frameGravity = _data.gravity * _gravityMultiplier * _core.GravityScale;
        _core.Velocity += Vector2.down * _frameGravity;
    }

    public float GetGravity()
    {
        return _frameGravity;
    }
}
