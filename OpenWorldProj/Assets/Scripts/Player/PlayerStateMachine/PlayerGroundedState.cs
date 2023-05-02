using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
    : base(currentContext, playerStateFactory) { }
    public override void  EnterState()
    {
        _ctx.CurrentMovementY = _ctx.GroundedGravity;
        _ctx.AppliedMovementY = _ctx.GroundedGravity;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsJumpedPressed && !_ctx.RequireNewJumpPress)
        {
            SwitchStates(_factory.Jump());
        }
    }

    public override void InitializeSubState()
    {

    }
}
