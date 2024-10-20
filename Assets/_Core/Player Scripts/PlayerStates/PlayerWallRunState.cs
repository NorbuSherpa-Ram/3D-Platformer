using UnityEngine;

public class PlayerWallRunState : PlayerWallState
{
    private Vector3 wallRunDirection;

    public PlayerWallRunState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.gravity = player.gravity * 0.5f;
        player.soundManager.PlayLoopSFX(player.runningSfx);

    }

    public override void Update()
    {
        base.Update();

        if ((player.IsTouchingLeftWall() || player.IsTouchingRightWall()) && Input.GetKeyDown(KeyCode.Space))
        {
            playerStateMachine.ChangeState(player.wallJumpState);
        }


        if ((player.IsTouchingLeftWall() || player.IsTouchingRightWall()) && !player.IsPlayergrounded())
        {
            wallRunDirection = Vector3.Cross(wallNormal, Vector3.up);

            if (Vector3.Dot(wallRunDirection, player.transform.forward) < 0)
                wallRunDirection *= -1;

            if (wallRunDirection.magnitude > 0.1f)
                player.transform.forward = wallRunDirection;

            Vector3 wallRunMovement = wallRunDirection * yInput * 10;

            characterController.Move(wallRunMovement * Time.deltaTime);
        }

        // WE ARE NOT PRESSING W OR UP ARROW 
        if ((!player.IsTouchingLeftWall() && !player.IsTouchingRightWall()) || yInput < 1 || player.IsPlayergrounded())
            playerStateMachine.ChangeState(player.idleState);
    }



    public override void Exit()
    {
        base.Exit();
        player.gravity = -9.81f;
        player.soundManager.StopLoopSFX(player.runningSfx);
    }
}
