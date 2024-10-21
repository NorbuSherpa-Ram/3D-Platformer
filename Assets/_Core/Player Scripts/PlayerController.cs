using System;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("COMPONENT INFO")]
    public Animator m_Animator { get; private set; }
    public CharacterController m_CharacterController { get; private set; }
    public PlayerSoundManager soundManager { get; private set; }

    [Space(20)]
    [Header("MOVEMENT INFO")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airMoveSpeed = 10f;

    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    [HideInInspector] public Vector3 velocity;

    public bool canCayoteJump { get; private set; }
    public float cayoteTimeDuration = 0.2f;
    public float jumpBufferDuration = .2f;


    [Space(20)]
    [Header("GROUNDED INFO")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGroudLayer;


    [Space(20)]
    [Header("WALL INFO")]
    public Transform wallCheckPoint;
    public LayerMask whatIsWall;
    public float wallCheckRange = 0.1f;
    public Vector3 wallJumpForce = new Vector3(10, 15f);
    public float wallJumpMovementCD = 0.5f;

    public float rotateSpeed = 5f; //SEED OF ROTATAION TOWARD MOVING 


    [Space(20)]
    [Header("CELLING INFO")]
    public Transform cellingCheckPoint;
    public LayerMask whatIsCelling;
    public float cellingCheckRange = 2;

    private RaycastHit leftWallHitInfo; //STORE HIT INFO 
    private RaycastHit rightWallHitInfo;

    [Space(20)]
    [Header("DASH RUN INFO")]
    public float dashSpeed = 10;
    public float dashDuration = 0.7f;
    public float dashCooldownTime = 10f;
    private float lastDashTimer;


    [Space(20)]
    [Header("SLIDE INFO ")]
    public float slideDuration = 1;
    public float slidingSpeed = 10;
    public float slideCooldownTime = 10f;
    private float lastSlideTime;
    public float characterDefaultHeght { get; private set; }
    //HEIGHT OF CHARACTER COLLIDER/CHARACTERCONTOLLER ON DASH  SO THAT PLAYER PASS THROUGH SMALL GAPS 
    public float slidingCharacterHeight = 0.7f;
    public Vector3 characterDefaultCenter { get; private set; }
    public Vector3 slidingCharacterCenter = new Vector3(0, 0.35f, 0);


    [Space(20)]
    [Header("FEEDBACK INFO ")] //MORE MOUNTAIN FEEL FEED BACK THAT GET TRIGGER ON EVENT SUCH AS DASH ETC 
    [SerializeField] private MMF_Player dashFeedback;
    [SerializeField] private MMF_Player slideFeedback;
    public MMF_Player runFeedback;


    [Space(20)]
    [Header("SOUND INFO ")]
    public SoundData walkingSfx;
    public SoundData runningSfx;
    [Space(5)]
    public SoundData jumpingSfx;
    public SoundData landSfx;
    [Space(5)]
    public SoundData dahsSfx;
    public SoundData slideSfx;



    #region EVENTS INFO 

    //FOR UI COOL DOWN VISUAL 
    public event EventHandler<SkillEventData> OnDash;
    public event EventHandler<SkillEventData> OnSlide;
    #endregion

    #region STATE INFO 
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallingState fallingState { get; private set; }
    public PlayerWallRunState wallRunState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }
    public PlayerDashState dashState { get; internal set; }
    public PlayerSlideState slideState { get; internal set; }


    public PlayerStateMachine stateMachine { get; private set; }

    #endregion

    private void Awake()
    {
        #region COMPONENT INITIALIZATION 
        m_Animator = GetComponentInChildren<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        soundManager = GetComponent<PlayerSoundManager>();
        #endregion

        characterDefaultCenter = m_CharacterController.center;
        characterDefaultHeght = m_CharacterController.height;

        #region STATE INITIALIZATION 
        stateMachine = new PlayerStateMachine();

        //PARAM  PLAYER REFERANCE , STATEMACHINE AND ANIMATION TO PLAY 
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
        runState = new PlayerRunState(this, stateMachine, "Run");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallingState = new PlayerFallingState(this, stateMachine, "Jump");
        wallRunState = new PlayerWallRunState(this, stateMachine, "Run");
        wallJumpState = new PlayerWallJump(this, stateMachine, "WallJump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        slideState = new PlayerSlideState(this, stateMachine, "Slide");

        #endregion
    }


    private void Start()
    {
        stateMachine.Initilization(idleState);
    }

    private void Update()
    {
        stateMachine.ExecuteState();
        ApplyGravity();
        Debug.Log(IsPlayergrounded());
    }



    //CUSTOM GRAVITY ALSO USED FOR JUMPE ,  DASH , SLIDE BY SETTING VELOCITY 
    private void ApplyGravity()
    {
        if (IsPlayergrounded() && velocity.y < -0.5f)
            velocity.y = -0.5f;
        else
            velocity.y += gravity * Time.deltaTime;

        m_CharacterController.Move(velocity * Time.deltaTime);
    }



    //TO MOVE 
    public void ApplyVelocity(Vector3 moveDirection)
    {
        m_CharacterController.Move(moveDirection * Time.deltaTime);
    }


    //TO ROTATE PLAYER TO WARD MOVING DIRECTION 
    public void PlayerLookAtRotation(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    public Vector3 GetCameraForward()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        return camForward;
    }
    public Vector3 GetCameraRight()
    {
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        return camRight;
    }





    public bool IsPlayergrounded() => Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGroudLayer);
    public bool IsTouchingCelling() => Physics.Raycast(cellingCheckPoint.position, Vector3.up, cellingCheckRange, whatIsCelling);

    public bool IsTouchingLeftWall() => Physics.Raycast(wallCheckPoint.position, Vector3.left, out leftWallHitInfo, wallCheckRange, whatIsWall);
    public bool IsTouchingRightWall() => Physics.Raycast(wallCheckPoint.position, Vector3.right, out rightWallHitInfo, wallCheckRange, whatIsWall);

    public RaycastHit GetLeftWallHit() => leftWallHitInfo;
    public RaycastHit GetRightWallHit() => rightWallHitInfo;

    public void AllowCayoteJump() => canCayoteJump = true;
    public void ResetCayoteJump() => canCayoteJump = false;


    #region SKILL COOLDOWN INFO 

    //CHECK FOR CAN DASH OR NOT IF IT CAN THEN DO CALL DASH
    //TO UPDATE LAST DASH TIME AND COOL DOWN UI 
    public bool CanDash()
    {
        return Time.time >= lastDashTimer + dashCooldownTime;
    }

    public bool CanSlide()
    {
        return Time.time >= lastSlideTime + slideCooldownTime;
    }

    public void Dash()
    {
        lastDashTimer = Time.time;
        OnDash?.Invoke(this, GetSkillData(dashCooldownTime, dashDuration));
        dashFeedback?.PlayFeedbacks();
    }

    public void Slide()
    {
        lastSlideTime = Time.time;
        OnSlide?.Invoke(this, GetSkillData(slideCooldownTime, slideDuration));
        slideFeedback?.PlayFeedbacks();
    }

    #endregion



    //RETURN EVENT ARGS CLASS FOR SKILL DATA SUCH  AS CD 
    private SkillEventData GetSkillData(float cooldownValue, float skillDuration)
    {
        SkillEventData args = new SkillEventData();
        args.coolDownTime = cooldownValue;
        args.skillDuration = skillDuration;
        return args;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (wallCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheckPoint.position, Vector2.right * wallCheckRange);
            Gizmos.DrawRay(wallCheckPoint.position, -Vector2.right * wallCheckRange);
        }
        Gizmos.DrawRay(cellingCheckPoint.position, Vector2.up * cellingCheckRange);

    }

}
public class SkillEventData : EventArgs
{
    public float coolDownTime;
    public float skillDuration;
}
