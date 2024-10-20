using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
    public float cooldownDuration = 2f;
    private float lastFeedbackTime = -Mathf.Infinity;

    public PlayerRunState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.soundManager.PlayLoopSFX(player.runningSfx);
        running = true;
    }

    public override void Update()
    {
        base.Update();


        if ((xInput == 0 && yInput == 0) || Input.GetKeyUp(KeyCode.LeftShift))
            playerStateMachine.ChangeState(player.idleState);

        Vector3 moveDir = (player.GetCameraForward() * yInput + player.GetCameraRight() * xInput).normalized;

        if (moveDir.magnitude > 0.1f)
            player.PlayerLookAtRotation(moveDir);


        Vector3 movePower = moveDir * player.runSpeed;
        player.ApplyVelocity(movePower);

        if (Time.time >= lastFeedbackTime + cooldownDuration)
        {
            player.runFeedback?.PlayFeedbacks();
            lastFeedbackTime = Time.time;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.runFeedback?.StopFeedbacks();
        player.soundManager.StopLoopSFX(player.runningSfx);
        running = false;
    }
}
