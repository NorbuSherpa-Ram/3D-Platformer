using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController player;

    private string animBoolName;

    protected CharacterController characterController;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;


    protected bool triggerCall;

    public PlayerState(PlayerController _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.playerStateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        player.m_Animator.SetBool(animBoolName, true);
        characterController = player.m_CharacterController;
        triggerCall = false;
    }
    public virtual void Update()
    {
        if (stateTimer > 0)
            stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.m_Animator.SetFloat("yVelocity", characterController.velocity.y);


    }
    public virtual void Exit()
    {
        player.m_Animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCall = true;
    }
}
