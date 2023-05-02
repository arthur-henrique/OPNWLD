using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        HandleGravity();
    }

    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsJumpingHash, false);
        if(_ctx.IsJumpedPressed)
        {
            _ctx.RequireNewJumpPress = true;
        }
        _ctx.CurrentJumpResetRoutine = _ctx.StartCoroutine(IJumpResetRoutine());
        if (_ctx.JumpCount == 3)
        {
            _ctx.JumpCount = 0;
            _ctx.Animator.SetInteger(_ctx.JumpCountHash, _ctx.JumpCount);
        }
    }

    public override void CheckSwitchState()
    {
        Debug.Log(_ctx.CharacterController.isGrounded);
        if (_ctx.CharacterController.isGrounded)
        {
            Debug.Log(_ctx.CharacterController.isGrounded);
            SwitchStates(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {

    }

    void HandleJump()
    {
        if (_ctx.JumpCount < 3 && _ctx.CurrentJumpResetRoutine != null)
        {
            _ctx.StopCoroutine(_ctx.CurrentJumpResetRoutine);
        }
        _ctx.Animator.SetBool(_ctx.IsJumpingHash, true);
        _ctx.IsJumping = true;
        _ctx.JumpCount += 1;
        _ctx.Animator.SetInteger(_ctx.JumpCountHash, _ctx.JumpCount);
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocites[_ctx.JumpCount];
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocites[_ctx.JumpCount];
    }

    void HandleGravity()
    {
        bool isFalling = _ctx.CurrentMovementY <= 0.0f || !_ctx.IsJumpedPressed;
        float fallMultiplier = 2f;


        if (isFalling)
        {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.JumpGravities[_ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * 0.5f, -20f);
        }
        else
        {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.JumpGravities[_ctx.JumpCount] * Time.deltaTime);
            _ctx.AppliedMovementY = (previousYVelocity + _ctx.CurrentMovementY) * 0.5f;
        }
    }

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _ctx.JumpCount = 0;
    }
}
