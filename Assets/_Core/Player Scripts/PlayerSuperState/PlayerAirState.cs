using UnityEngine;


/// <summary>
/// SUPER CLASS FOR JUMP AND  FALLING CLASS 
/// MAINLY RESPONSIBLE FOR TAKING INPUT DURING AIR STATE 
/// </summary>

public class PlayerAirState : PlayerState
{
    private float cayoteTimer;
    private float jumpBufferTimer;


    public PlayerAirState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        cayoteTimer = player.cayoteTimeDuration; // Start coyote timer when entering air state
        Debug.Log(" Enter Air State");
    }

    public override void Update()
    {
        base.Update();

        CayoteJumpAndJumpBuffer();
        Move();

        if ((player.IsTouchingLeftWall() || player.IsTouchingRightWall()) && !player.IsPlayergrounded() && yInput > 0)
            playerStateMachine.ChangeState(player.wallRunState);

        if (player.IsPlayergrounded() && jumpBufferTimer <= 0)
        {
            Debug.Log("Enter Enter Idle State  State ");

            player.soundManager.PlayOneShotSFX(player.landSfx);
            playerStateMachine.ChangeState(player.idleState);
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
    //ALLOW AIR MOVEMNET    
    protected void Move()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDir = (camForward * yInput + camRight * xInput).normalized;

        if (moveDir.magnitude > 0.1f)
            player.PlayerLookAtRotation(moveDir);

        Vector3 movePower = moveDir * player.airMoveSpeed;
        player.ApplyVelocity(movePower);
    }



    /// <summary>
    /// CHEECK FOR  CAYOTE AND  JUMP BUFFER TIMER 
    /// </summary>
    private void CayoteJumpAndJumpBuffer()
    {
        if (cayoteTimer > 0)
            cayoteTimer -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cayoteTimer > 0 && player.canCayoteJump)
            {
                playerStateMachine.ChangeState(player.jumpState);
                //DO NOT ALLOW CAYOTE JUMP ONCE USED TILL IT RESET 
                player.ResetCayoteJump();
            }
            else
                jumpBufferTimer = player.jumpBufferDuration;
        }


        jumpBufferTimer -= Time.deltaTime;

        if (player.IsPlayergrounded() && jumpBufferTimer > 0)
            playerStateMachine.ChangeState(player.jumpState);
    }


}
