using UnityEngine;

public class PlayerDashState : PlayerState
{
    private Vector3 dashDirection;
    public PlayerDashState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        dashDirection = player.transform.forward;

        //BECAUSE OF EVENT IT IS CALLED LAST 
        player.Dash();
        player.soundManager.PlayOneShotSFX(player.dahsSfx);
    }

    public override void Update()
    {
        base.Update();

        player.velocity.y = 0f;
        player.velocity = dashDirection * player.dashSpeed;

        if (stateTimer < 0)
            playerStateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.velocity.x = 0;
        player.velocity.z = 0;
    }

}
