

using UnityEngine;

public class PlayerAirState : PlayerState
{
    //TREEACJ COYOTE TIME AND JUMP BUFFER 
    private float coyoteTimeCounter; 
    private float jumpBufferCounter;


    public PlayerAirState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        coyoteTimeCounter = player.cayoteTimeDuration; // Start coyote time when entering air state
    }

    public override void Update()
    {
        base.Update();

        CayoteJumpAndJumpBuffer();
        Move();

        if ((player.IsTouchingLeftWall() || player.IsTouchingRightWall()) && !player.IsPlayergrounded() && yInput > 0)
            playerStateMachine.ChangeState(player.wallRunState);



        if (player.IsPlayergrounded() && jumpBufferCounter <= 0)
        {
            player.soundManager.PlayOneShotSFX(player.landSfx);
            playerStateMachine.ChangeState(player.idleState);

        }


        //if (player.IsPlayergrounded())
        //{
        //    player.soundManager.PlayOneShotSFX(player.landSfx);
        //    playerStateMachine.ChangeState(player.idleState);
        //}

    }

    protected  void Move()
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

    private void CayoteJumpAndJumpBuffer()
    {
        if (coyoteTimeCounter > 0)
            coyoteTimeCounter -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (coyoteTimeCounter > 0 && player.canCayoteJump)
            {
                playerStateMachine.ChangeState(player.jumpState);
                //DO NOT ALLOW CAYOTE JUMP ONCE USED TILL IT RESET 
                player.ResetCayoteJump();
            }
            else
                jumpBufferCounter = player.jumpBufferDuration;
        }


        jumpBufferCounter -= Time.deltaTime;

        if (player.IsPlayergrounded() && jumpBufferCounter > 0)
            playerStateMachine.ChangeState(player.jumpState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
