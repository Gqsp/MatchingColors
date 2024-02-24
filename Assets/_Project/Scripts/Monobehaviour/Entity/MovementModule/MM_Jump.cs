using System;
using System.Collections.Generic;
using GaspDL.Audio;
using UnityEngine;

public class MM_Jump : EntityMovementModule<JumpProfile>
{
    [SerializeField] EntityStateResolver GroundedResolver;
    [SerializeField] EntityStateResolver AirbornResolver;
    [SerializeField] ParticleSystem _landParticle;
    [SerializeField] LayerMask _orangeLayer;

    float _lastPerformedInput;
    float _lastGrounded;
    float _startJumpTime;
    
    bool _jumping;
    float _attenuatedJumpForce;
    float _lastFallingYVelocity;

    public override List<EntityStateResolver> GetUsedStates()
    {
        return new List<EntityStateResolver> { GroundedResolver, AirbornResolver };
    }

    private JumpProfile _data;

    private void Start()
    {
        _data = _profiles.GetProfile();
        PlayerDeathHandler.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerDeathHandler.OnPlayerDeath -= OnPlayerDeath;
    }

    public override void UpdateModule()
    {
        if (!_core.TryGetState(GroundedResolver, out var grounded)) return;
        if (!_core.TryGetState(AirbornResolver, out var airborn)) return;

        var inputData = _core.ActionData;

        bool pressingJump = inputData.Performed(EntityActionRequest.Jump);
        bool startedJump = pressingJump && !inputData.PerformedLastFrame(EntityActionRequest.Jump);
        if (startedJump) _lastPerformedInput = Time.fixedTime;
        
        bool onOrange = false;
        
        if (grounded.validThisFrame && _core.LastFrameVelocity.y <= 1f)
        {
            _overrideJump = false;
            onOrange = _core.FrameCollisionData.CheckCollisionDown(_orangeLayer.value);
            
            if (!onOrange) _lastGrounded = Time.fixedTime;

            if (!grounded.validPreviousFrame)
            {
                AudioManager.Instance.PlaySound("Land");
                _jumping = false;
                
                if (airborn.validPreviousFrame)
                    _core.ParticleHandler.SpawnParticle(Vector2.down, _landParticle, Quaternion.identity);

                _attenuatedJumpForce = 0;
                if (!onOrange) _lastFallingYVelocity = 0;
            }
        }
        else
        {
            _lastFallingYVelocity = Mathf.Min(_lastFallingYVelocity, _core.LastFrameVelocity.y);
        }

        if (_jumping && startedJump && !airborn.validThisFrame && _forceJumping <= 0)
        {
            _jumping = false;
        }
        
        if (_jumping)
        {
            if (_core.FrameCollisionData.CheckCollisionUp())
            {
                _startJumpTime = 0;
                _lastFallingYVelocity = 0;
                _attenuatedJumpForce = _core.GetGravity();
            }

            if (pressingJump && _startJumpTime + (_overrideJump ? _overrideJumpTime : _data.jumpCutOff) - Time.fixedTime > -0.01f)
            {
                _lastFallingYVelocity = 0;
                _attenuatedJumpForce = _overrideJump ? _overrideJumpForce : _data.jumpForce;
            }
            else
            {
                _startJumpTime = 0;
                _attenuatedJumpForce = Mathf.Max(_attenuatedJumpForce - (_overrideJump ? _overrideJumpAttenuation : _data.jumpAttenuation) * Time.fixedDeltaTime,
                    _data.downwardForce * _core.GravityScale);
            }
            
            _core.Velocity += Vector2.up * _attenuatedJumpForce;
        }
        else
        {
            if (_lastPerformedInput + _data.jumpBuffer >= Time.fixedTime &&
                _lastGrounded + _data.coyoteTime >= Time.fixedTime)
            {
                _jumping = true;
                _startJumpTime = Time.fixedTime;
                _attenuatedJumpForce = _data.jumpForce;
                _lastFallingYVelocity = 0;
                _core.AnimatorHandler.SetTrigger("Jump");
                AudioManager.Instance.PlaySound("Jump");
            }
        }
        
        if (onOrange && !_jumping)
        {
            _jumping = true;
            _startJumpTime = 0;

            _attenuatedJumpForce = Mathf.Abs(_lastFallingYVelocity) *
                                   _data.bounceCurve.Evaluate(Mathf.Abs(_lastFallingYVelocity));

            if (pressingJump) _attenuatedJumpForce = Mathf.Max(_attenuatedJumpForce, _data.minJumpForceBounceHoldJump);
            else _attenuatedJumpForce = Mathf.Max(_attenuatedJumpForce, _data.minJumpForceBounceNoHold);

            _core.AnimatorHandler.SetTrigger("Jump");
        }

        _forceJumping--;
    }
    
    int _forceJumping;
    bool _overrideJump;
    float _overrideJumpForce;
    float _overrideJumpTime;
    float _overrideJumpAttenuation;
    public void SetJumpForce(float force, float jumpTime, float verticalAttenuation )
    {
        _jumping = true;
        _overrideJump = true;
        
        _attenuatedJumpForce = force;
        _overrideJumpForce = force;
        _overrideJumpTime = jumpTime;
        _overrideJumpAttenuation = verticalAttenuation;
        
        // Frame delay to avoid the jump being cancelled by the next frame
        _forceJumping = 5;
        _lastFallingYVelocity = 0;
        _startJumpTime = Time.fixedTime;
        
        _core.AnimatorHandler.SetTrigger("Jump");
    }
    
    private void OnPlayerDeath()
    {
        _jumping = false;
        _attenuatedJumpForce = 0;
        _lastFallingYVelocity = 0;
        _startJumpTime = 0;
    }
}