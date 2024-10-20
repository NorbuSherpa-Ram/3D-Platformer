using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.soundManager.PlayLoopSFX(player.walkingSfx);
    }

    public override void Update()
    {
        base.Update();


        Vector3 moveDir = (player.GetCameraForward() * yInput + player.GetCameraRight() * xInput).normalized;

        if (moveDir.magnitude > 0.1f)
            player.PlayerLookAtRotation(moveDir);


        Vector3 movePower = moveDir * player.walkSpeed;
        player.ApplyVelocity(movePower);


        if (xInput == 0 && yInput == 0)
            playerStateMachine.ChangeState(player.idleState);
    }



    public override void Exit()
    {
        base.Exit();

        player.soundManager.StopLoopSFX(player.walkingSfx);
    }
}
