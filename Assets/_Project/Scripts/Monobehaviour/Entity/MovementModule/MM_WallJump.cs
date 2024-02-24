using System.Collections.Generic;
using UnityEngine;

public class MM_WallJump : EntityMovementModule<WallJumpProfile>
{
    [SerializeField] EntityStateResolver WallClingLeft, WallClingRight, Grounded;
    [SerializeField] ParticleSystem wallJumpParticles, wallSlideParticles;
    MM_Move _moveModule;
    MM_Jump _jumpModule;
    bool _isJumping;
    float _lastJumpInput;
    float _attenuatedJumpForce;

    WallJumpProfile _data;

    private void Start()
    {
        _moveModule = _abilities.QueryModule<MM_Move>();
        _jumpModule = _abilities.QueryModule<MM_Jump>();

        _data = _profiles.GetProfile();
    }

    public override List<EntityStateResolver> GetUsedStates()
    {
        return new List<EntityStateResolver> { WallClingLeft, WallClingRight, Grounded };
    }

    public override void UpdateModule()
    {
        if (!_core.TryGetState(WallClingLeft, out var leftWall)) return;
        if (!_core.TryGetState(WallClingRight, out var rightWall)) return;

        bool isWallClinged = leftWall.validThisFrame || rightWall.validThisFrame;
        bool wasWallClinged = leftWall.validPreviousFrame || rightWall.validPreviousFrame;

        var inputData = _core.ActionData;
        bool startedJump = inputData.Started(EntityActionRequest.Jump);
        if (startedJump && !_core.FrameCollisionData.CheckCollisionDown())
        {
            _lastJumpInput = Time.fixedTime;
        }

        _core.GravityScale = isWallClinged && _core.LastFrameVelocity.y < 0 ? _data.gravityScale : 1;

        if ((isWallClinged && !wasWallClinged) ||
            _core.TryGetState(Grounded, out var grounded) && grounded.validThisFrame)
        {
            _isJumping = false;
        }

        if (!_isJumping && _lastJumpInput + _data.jumpBuffer > Time.fixedTime && isWallClinged)
        {
            _isJumping = true;
            _lastJumpInput = 0;
            
            Vector2 horizontalDir = leftWall.validThisFrame ? Vector2.right : Vector2.left;
            _moveModule.SetHorizontalForce(horizontalDir.x * _data.horizontalForce);
            
            _jumpModule.SetJumpForce(_data.verticalForce, _data.jumpCutOff, _data.verticalAttenuation);
            
            _core.ParticleHandler.SpawnParticle(horizontalDir * -1, wallJumpParticles,
                Quaternion.Euler(0, leftWall.validThisFrame ? 0 : 180, 0));
        }

        // Wall Slide Particles
        if (isWallClinged && !wasWallClinged)
        {
            Vector2 particleDirection = leftWall.validThisFrame ? Vector2.left : Vector2.right;
            Quaternion particleRotation = Quaternion.Euler(0, leftWall.validThisFrame ? 0 : 180, 0);
            _core.ParticleHandler.SpawnLoopedParticle(particleDirection, wallSlideParticles, particleRotation);
        }
        else if (!isWallClinged && wasWallClinged)
        {
            _core.ParticleHandler.StopLoopedParticle(wallSlideParticles);
        }
    }
}