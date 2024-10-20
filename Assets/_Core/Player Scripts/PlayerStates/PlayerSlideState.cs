using UnityEngine;

public class PlayerSlideState : PlayerState
{
    public PlayerSlideState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        characterController.center = player.slidingCharacterCenter;
        characterController.height = player.slidingCharacterHeight;
        stateTimer = player.slideDuration;

        player.soundManager.PlayOneShotSFX(player.slideSfx);

        player.Slide();
    }



    public override void Update()
    {
        base.Update();
        player.velocity = player.transform.forward * player.slidingSpeed;

        //MEAN IT PLAYER IS TOUCHING CELLING THEN SLIDE HIM LITTLE FORWARD
        if (stateTimer < 0 && !player.IsTouchingCelling())
            playerStateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.velocity = Vector3.zero;
        characterController.center = player.characterDefaultCenter;
        characterController.height = player.characterDefaultHeght;

    }
}
