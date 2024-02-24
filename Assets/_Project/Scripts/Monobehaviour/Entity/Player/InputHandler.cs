using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : EntityComponent
{
    PlayerInputs _inputs;

    EntityActionData _actionData;
    public EntityActionData ActionData => _actionData;

    private int _consumeThisFrame;
    private bool _lockInputs;
    public bool LockInputs
    {
        set
        {
            _lockInputs = value;
            if (_lockInputs) _inputs.Disable();
            else _inputs.Enable();
        }
    }

    private void Start()
    {
        _inputs = new PlayerInputs();
        _inputs.Enable();
        _inputs.Gameplay.Move.performed += Move;
        _inputs.Gameplay.Move.canceled += Move;
        _inputs.Gameplay.Jump.started += JumpStart;
        _inputs.Gameplay.Jump.canceled += JumpEnd;
    }

    private void FixedUpdate()
    {
        _actionData.previousFrameInputs = _actionData.actionRequests;
        _actionData.actionRequests &= ~_consumeThisFrame;
        _consumeThisFrame = 0;
    }

    #region Input Events
    void Move(InputAction.CallbackContext ctx)
    {
        _actionData.movement = ctx.ReadValue<Vector2>().x;
    }

    void JumpStart(InputAction.CallbackContext ctx)
    {
        _actionData.actionRequests |= 1 << (int)EntityActionRequest.Jump;
    }

    void JumpEnd(InputAction.CallbackContext ctx) { 
        _consumeThisFrame  |= 1 << (int)EntityActionRequest.Jump;
    }
    #endregion
}
