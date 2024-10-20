using UnityEngine;

public class PlayerWallState : PlayerState
{
    protected Vector3 wallNormal;



    public PlayerWallState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        wallNormal = player.IsTouchingLeftWall() ? player.GetLeftWallHit().normal : player.GetRightWallHit().normal;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
