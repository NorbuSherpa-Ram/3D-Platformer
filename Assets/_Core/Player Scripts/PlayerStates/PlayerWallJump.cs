using UnityEngine;

public class PlayerWallJump : PlayerWallState
{
    //
    private float lastJumpTime;

    public PlayerWallJump(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        float jumpXDirection = player.IsTouchingLeftWall() ? 1 : -1;
        Vector3 jumpVelocity = new Vector3(jumpXDirection * player.wallJumpForce.x, player.wallJumpForce.y, characterController.velocity.z);
        player.velocity = jumpVelocity;
        lastJumpTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (characterController.velocity.y < 0)
        {
            playerStateMachine.ChangeState(player.fallingState);
        }

        //STOP MOVEMENT IF I JUST JUMPED 
        if (CanMove())
        {
            if ((player.IsTouchingLeftWall() || player.IsTouchingRightWall()) && !player.IsPlayergrounded() && yInput > 0)
                playerStateMachine.ChangeState(player.wallRunState);

            Move();
        }
    }


    public override void Exit()
    {
        base.Exit();
        player.velocity.x = 0;
        lastJumpTime = 0;
    }


    public bool CanMove()
    {
        return lastJumpTime + player.wallJumpMovementCD <= Time.time;
    }

    private void Move()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDir = (camForward * yInput + camRight * xInput).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            player.transform.forward = moveDir;
        }
        Vector3 movePower = moveDir * player.airMoveSpeed;
        player.ApplyVelocity(movePower);
    }
}
