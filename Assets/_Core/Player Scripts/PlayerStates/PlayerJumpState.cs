using DG.Tweening;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{

    public PlayerJumpState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.velocity.y = 0;
        player.velocity.y = Mathf.Sqrt(player.jumpHeight * -2f * player.gravity);
        player.soundManager.PlayOneShotSFX(player.jumpingSfx);


        Debug.Log("Enter Jump State ");
    }

    public override void Update()
    {
        base.Update();

        if (characterController.velocity.y < 0)
            playerStateMachine.ChangeState(player.fallingState);

    }

    public override void Exit()
    {
        base.Exit();

    }
}
