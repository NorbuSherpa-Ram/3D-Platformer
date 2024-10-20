using Codice.CM.Common;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls playerControls;
        [SerializeField] private CharacterController characterController;


        [Header("MOVEMENT INFO")]
        [SerializeField] private float walkSpeed = 100f;
        [SerializeField] private float runSpeed = 200f;
        [SerializeField] private float moveSpeed;

        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundGravity = -0.5f;
        [SerializeField] private float jumpHeight = 100f;
        private Vector3 verticalVelocity;

        [Space(20)]
        [Header("GROUNDED INFO")]
        [SerializeField] private float dashSpeed = 10;
        [SerializeField] private float dashDuration = 1;
        [SerializeField] private float dashTimer;
        [SerializeField] private bool isDashing;

        [Space(20)]
        [Header("ANIMATOR INFO")]
        [SerializeField] private Animator animator;


        [Space(20)]
        [Header("GROUNDED INFO")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private LayerMask whatIsGroudLayer;

        private bool isInAir;


        [SerializeField] private Transform cam;


        [Space(20)]
        [Header("WALL RUN INFO")]
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private LayerMask whatIsWall;

        [SerializeField] private float wallRunSpeed;
        [SerializeField] private float wallCheckRange;
        [SerializeField] private float minHightForWallRun = 1f;

        private RaycastHit isLeftWallHit;
        private RaycastHit isRightWallHit;

        [SerializeField] private bool onRightWall;
        [SerializeField] private bool onLeftWall;
        [SerializeField] private bool isWallRunning;
        [SerializeField] private bool isWallJumping;

        private Vector3 wallNormal;
        private Vector3 wallRunDirection;



        [Space(20)]
        [Header("WALL JUMP INFO")]
        [SerializeField] private Vector3 wallJumpForce = new Vector3(1, 1.5f, 0);


        [Space(20)]
        [Header("SLIDE INFO ")]
        [SerializeField] private float slideDuration;
        [SerializeField] private float slidingSpeed = 10;
        private float slideTimer;
        [SerializeField] private bool isSliding = false;

        private Vector3 characterControllerDefaultCenter;
        private float characterControllerDefaultHight;

        [SerializeField] private Vector3 slidingCharacterCenter;
        [SerializeField] private float slidingCharacterControllerHeight;





        private Vector2 inputDirection;
        private bool isJumping;

        private void Awake()
        {
            playerControls = new PlayerControls();

            characterControllerDefaultCenter = characterController.center;
            characterControllerDefaultHight = characterController.height;
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }




        private void Update()
        {
            ApplyGravity();


            inputDirection = playerControls.Character.Movement.ReadValue<Vector2>();

            Dash();

            if (isDashing) return;


            Slide();
            if (isSliding) return;

            if (Input.GetKeyDown(KeyCode.Space) && isWallRunning)
            {
                WallJump();
            }

            if (isWallJumping)
                return;

            CheckWallRun();
            if (isWallRunning)
            {
                WallRunningMovement();
                return;
            }


            HandleJump();
            HandleMovement();
            //ApplyGravity();
        }

        private void Dash()
        {
            if (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
            }

            if (dashTimer <= 0)
            {
                isDashing = false;
            }

            if (Input.GetKeyDown(KeyCode.F) && dashTimer <= 0 && !isDashing)
            {
                isDashing = true;

                dashTimer = dashDuration;
                animator.SetTrigger("isDashing");
            }

            if (isDashing)
            {
                characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
            }
        }

        private void Slide()
        {
            if (slideTimer <= 0 && !isSliding && Input.GetKeyDown(KeyCode.C))
            {
                isSliding = true;
                slideTimer = slideDuration;
                animator.SetTrigger("isSliding");
            }

            if (slideTimer > 0)
                slideTimer -= Time.deltaTime;


            if (slideTimer <= 0 && isSliding)
            {
                isSliding = false;
                DOVirtual.DelayedCall(1, () =>
                {
                    characterController.center = characterControllerDefaultCenter;
                    characterController.height = characterControllerDefaultHight;
                });
            }

            if (isSliding)
            {
                characterController.center = slidingCharacterCenter;
                characterController.height = slidingCharacterControllerHeight;
                characterController.Move(transform.forward * slidingSpeed * Time.deltaTime);
            }
        }




        #region MOVEMENT

        private void HandleMovement()
        {
            float horInput = inputDirection.x;
            float verInput = inputDirection.y;

            animator.SetFloat("xVelocity", horInput);
            animator.SetFloat("zVelocity", verInput);

            //Vector3 moveDir = new Vector3(horInput, 0f, verInput).normalized;
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            //STOP ROTAION IN Y DIRECTION 
            camForward.y = 0f;
            camRight.y = 0f;

            Vector3 moveDir = (camForward * verInput + camRight * horInput).normalized;

            if (moveDir.magnitude > 0.1f)
            {
                transform.forward = moveDir;
            }

            HandleRunning();

            Vector3 movePower = moveDir * moveSpeed * Time.deltaTime;
            characterController.Move(movePower);
        }

        private void HandleRunning()
        {
            bool isRunning = !isInAir && Input.GetKey(KeyCode.LeftShift) && verticalVelocity.magnitude > 0;
            moveSpeed = isRunning ? runSpeed : walkSpeed;
            animator.SetBool("isRunning", isRunning);
        }

        private void ApplyGravity()
        {
            if (IsPlayergrounded() && verticalVelocity.y < 0.2f)
            {
                verticalVelocity.y = groundGravity;
                animator.SetBool("isJump", false);
                isInAir = false;
            }
            else
            {
                verticalVelocity.y += gravity * Time.deltaTime;
            }
            characterController.Move(verticalVelocity * Time.deltaTime);
        }
        #endregion

        #region JUMPING

        private void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetBool("isJump", true);
            }

            if (!IsPlayergrounded())
                isInAir = true;

            //Check If Not in Ground And Falling 
            //animator.SetBool("isJump", isInAir);

            animator.SetFloat("yVelocity", verticalVelocity.y);
        }
        #endregion

        private void OnDisable()
        {
            playerControls.Disable();
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
        }

        #region OLD WALL RUN 

        private void CheckWallRun()
        {
            onLeftWall = Physics.Raycast(wallCheckPoint.position, Vector3.left, out isLeftWallHit, wallCheckRange, whatIsWall);
            onRightWall = Physics.Raycast(wallCheckPoint.position, Vector3.right, out isRightWallHit, wallCheckRange, whatIsWall);

            if ((onLeftWall || onRightWall) && !isWallRunning && CanWallRun())
            {
                StartWallRunning();
                animator.SetTrigger("wallRun");
                animator.SetBool("isWallRunning", true);
            }
            if (((!onLeftWall && !onRightWall) && isWallRunning) || !CanWallRun())
            {
                ExitWallRunning();
            }
        }

        private bool CanWallRun()
        {
            float verticalAxis = Mathf.Abs(inputDirection.x);
            //check bool for running 
            return !IsPlayergrounded() && verticalAxis > 0 && VerticalCheck()/* && isSprinting*/;
        }
        private void StartWallRunning()
        {
            isWallRunning = true;

            wallNormal = onLeftWall ? isLeftWallHit.normal : isRightWallHit.normal;
            wallRunDirection = Vector3.Cross(wallNormal, Vector3.up);

            if (Vector3.Dot(wallRunDirection, transform.forward) < 0)
                wallRunDirection *= -1;

            if (wallRunDirection.magnitude > 0.1f)
                transform.forward = wallRunDirection;

            gravity = gravity * 0.5f;
        }

        private void WallRunningMovement()
        {
            float verInput = inputDirection.y;
            Vector3 wallRunMovement = wallRunDirection * verInput * wallRunSpeed;
            characterController.Move(wallRunMovement * Time.deltaTime);


            if (verInput < 0.1f)
            {
                ExitWallRunning();
            }
        }

        private bool VerticalCheck()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minHightForWallRun);
        }
        private void ExitWallRunning()
        {
            isWallRunning = false;
            gravity = -9.81f;
            animator.SetBool("isWallRunning", false);
        }

        private void WallJump()
        {
            animator.SetBool("isJump", true);
            isWallJumping = true;

            verticalVelocity.y = 0;
            wallNormal = onLeftWall ? isLeftWallHit.normal : isRightWallHit.normal;

            Vector3 jump = new Vector3(wallNormal.x * wallJumpForce.x, Mathf.Sqrt(wallJumpForce.y * -2f * gravity));
            verticalVelocity = jump;

            Invoke(nameof(ResetWallJump), 0.5f);
        }

        private void ResetWallJump()
        {
            animator.SetBool("isJump", false);
            isWallJumping = false;
            verticalVelocity.x = 0;
        }
        #endregion
        private bool IsPlayergrounded() => Physics.CheckSphere(groundCheck.position, groundCheckRadius);
    }
}
