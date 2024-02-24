using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MM_Move : EntityMovementModule<MovementProfile>
{
    [SerializeField] EntityStateResolver AirbornResolver, GroundedResolver;
    [SerializeField] ParticleSystem _walkParticle;
    [SerializeField] private LayerMask _blueLayer;
    [SerializeField] private LayerMask _orangeLayer;

    public override List<EntityStateResolver> GetUsedStates()
    {
        return new List<EntityStateResolver> { AirbornResolver, GroundedResolver };
    }

    float _currentVelocity;
    float _lastFrameAirBorn;
    float _speedCap;

    MovementProfile _data;

    private void Start()
    {
        PlayerDeathHandler.OnPlayerDeath += OnPlayerDeath;
        _data = _profiles.GetProfile();
        _speedCap = _data.speedCap;
    }

    public override void UpdateModule()
    {
        if (!_core.TryGetState(AirbornResolver, out var airborn)) return;
        if (!_core.TryGetState(GroundedResolver, out var grounded)) return;
        var isOnOrange = _core.FrameCollisionData.CheckCollisionDown(_orangeLayer.value);
        var isOnIce = _core.FrameCollisionData.CheckCollisionDown(_blueLayer.value);
        if (airborn.validThisFrame) _lastFrameAirBorn = Time.fixedTime;

        float acceleration;

        if (!isOnOrange) _data = _profiles.GetProfile(isOnIce ? 1 : 0);
        
        if (grounded.validThisFrame)
        {
            acceleration = (int)Mathf.Sign(_core.ActionData.movement) != (int)Mathf.Sign(_currentVelocity)
                ? _data.turnAroundAcceleration
                : _data.acceleration;

            if (!isOnOrange)
            {
                _speedCap = _data.speedCap;
            }
        }
        else
        {
            acceleration = _data.airControl;
        }

        float animSpeed = isOnIce && _core.ActionData.movement != 0 ? 3 : 1;
        _core.AnimatorHandler.SetAnimSpeed(animSpeed);


        if (_forcedVelocity <= 0 && ((_core.FrameCollisionData.CheckCollisionLeft() &&
                                      (_currentVelocity < 0 || _core.ActionData.movement < 0)) ||
                                     (_core.FrameCollisionData.CheckCollisionRight() &&
                                      (_currentVelocity > 0 || _core.ActionData.movement > 0))))
        {
            _currentVelocity = 0;
        }
        else
        {
            if (_core.ActionData.movement != 0)
            {
                var curve = _data.accelerationLinkedToVelocity;
                var acelMult = curve.Evaluate(Mathf.Abs(_currentVelocity) / _data.groundedSpeed);
                _currentVelocity = Mathf.Lerp(_currentVelocity, _data.groundedSpeed * _core.ActionData.movement,
                    acceleration * acelMult * Time.fixedDeltaTime);
            }
            else
            {
                var deceleration = _data.deceleration;
                if (!airborn.validThisFrame && airborn.validPreviousFrame)
                {
                    deceleration = _data.deceleration * _data.decelerationCurveLinkedToAirbornVelocity.Evaluate(
                                            Mathf.Abs(_core.LastFrameVelocity.x));
                }

                var timeSinceGrounded = Time.fixedTime - _lastFrameAirBorn;

                deceleration = Mathf.Lerp(0, deceleration, timeSinceGrounded / _data.timeAfterAirbornToDecelerate);
                _currentVelocity = Mathf.Lerp(_currentVelocity, _data.groundedSpeed * _core.ActionData.movement,
                    deceleration * Time.fixedDeltaTime);
            }
            
            var cappedVelocity = Mathf.Clamp(_currentVelocity, -_speedCap, _speedCap);
            _core.Velocity += Perpendicular1(_core.FrameCollisionData.floorNormal) * cappedVelocity;
        }

        _forcedVelocity--;

        // Handle Particles
        if (grounded.validThisFrame && Mathf.Abs(_currentVelocity) > 0.1f)
        {
            _core.ParticleHandler.SpawnLoopedParticle(Vector2.down, _walkParticle,
                Quaternion.Euler(0, _currentVelocity < 0 ? 0 : 180, 0));
        }
        else
        {
            _core.ParticleHandler.StopLoopedParticle(_walkParticle);
        }
    }

    int _forcedVelocity;

    public void SetHorizontalForce(float force)
    {
        _currentVelocity = force;
        _forcedVelocity = 5;
    }

    private void OnPlayerDeath()
    {
        _currentVelocity = 0;
    }
    
    public Vector2 Perpendicular1(Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x);
    }
}