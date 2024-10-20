using UnityEngine;
public class PlayerGroundedState : PlayerState
{
    protected bool running;
    public PlayerGroundedState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (!player.IsPlayergrounded())
        {
            playerStateMachine.ChangeState(player.fallingState);
            player.AllowCayoteJump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsPlayergrounded())
            playerStateMachine.ChangeState(player.jumpState);


        if (Input.GetKeyDown(KeyCode.F) && player.IsPlayergrounded() && player.CanDash())
        {
            playerStateMachine.ChangeState(player.dashState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && player.IsPlayergrounded() && player.CanSlide())
        {
            playerStateMachine.ChangeState(player.slideState);
            return;
        }



        if ((xInput != 0 || yInput != 0) && Input.GetKey(KeyCode.LeftShift) && player.IsPlayergrounded() && !running)
            playerStateMachine.ChangeState(player.runState);

    }

    public override void Exit()
    {
        base.Exit();
    }




}
