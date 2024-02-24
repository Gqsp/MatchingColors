using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private EntityMovementCore _movementHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Animator _animator;
    
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    
    [SerializeField] AnimationCurve _scaleXRelativeToVerticalVelocity;
    [SerializeField] AnimationCurve _scaleYRelativeToVerticalVelocity;

    [SerializeField] Color _orange = new (1, 0.5f, 0);
    [SerializeField] Color _blue = new (0, 0.5f, 1);
    [SerializeField] Color _green = new (0, 1, 0.5f);
    
    float _smoothedVelocity;
    float _velocity;
    
    static readonly int AnimSpeed = Animator.StringToHash("AnimSpeed");
    static readonly int PaintColor = Shader.PropertyToID("_PaintColor");
    
    private void Start()
    {
        switch (GameData.startMode)
        {
            case StartMode.Normal:
                _spriteRenderer.material.SetColor(PaintColor, Color.white);
                break;
            case StartMode.Slippery:
                _spriteRenderer.material.SetColor(PaintColor, _blue);
                break;
            case StartMode.Bouncy:
                _spriteRenderer.material.SetColor(PaintColor, _orange);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        _spriteRenderer.flipX = _inputHandler.ActionData.movement switch
        {
            > 0 => false,
            < 0 => true,
            _ => _spriteRenderer.flipX
        };
        
        _animator.SetFloat(Velocity, Mathf.Abs(_movementHandler.LastFrameVelocity.x));
        
    }
    
    public void SetAnimSpeed(float speed)
    {
        _animator.SetFloat(AnimSpeed, speed);
    }

    private void FixedUpdate()
    {
        _smoothedVelocity = Mathf.SmoothDamp(_smoothedVelocity, _movementHandler.LastFrameVelocity.y, ref _velocity, 0.1f);
        transform.localScale = new Vector3(
            _scaleXRelativeToVerticalVelocity.Evaluate(_smoothedVelocity),
            _scaleYRelativeToVerticalVelocity.Evaluate(_smoothedVelocity),
            1);
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
