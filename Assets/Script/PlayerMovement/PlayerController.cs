using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float slideSpeed;
        [SerializeField] private float groundDrag;
        [SerializeField] private float speedIncreaseMulti;
        [SerializeField] private float slopeIncreaseMulti;
        [SerializeField] private float wallRunSpeed;

        private float moveSpeed;
        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMulti;

        private bool readyJump;

        [Header("Crouch")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float crouchYScale;
        
        private float startYScale;

        [Header("GroundCheck")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask whatIsGround;
        private bool grounded;

        [Header("Slope")]
        [SerializeField] private float maxSlopeAngle;

        private RaycastHit slopeHit;
        private bool exitSlope;

        [SerializeField] private Transform orientation;

        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDir;

        private Rigidbody rb;

        [SerializeField] private GameManager gameManage;


        //Player States
        public MovementState state;

        public enum MovementState
        {
            run,
            walk,
            crouch,
            slide,
            wallrunning,
            air
        }

        public bool isSliding;
        public bool isWallRun;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyJump = true;

            startYScale = transform.localScale.y;
        }

        // Update is called once per frame
        void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            PlayerInput();
            SpeedControl();
            StateHandler();

            //Apply drag
            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        private void PlayerInput()
        {
            if (InputManager.Instance.getMoveForward())
            {
                verticalInput = 1;
            }
            else if (InputManager.Instance.getMoveBackward())
            {
                verticalInput = -1;
            }
            else
            {
                verticalInput = 0;
            }

            if (InputManager.Instance.getMoveRight())
            {
                horizontalInput = 1;
            }
            else if (InputManager.Instance.getMoveLeft())
            {
                horizontalInput = -1;
            }
            else
            {
                horizontalInput = 0;
            }

            if (InputManager.Instance.getJump() && readyJump && grounded)
            {
                readyJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            //Start Crouch
            if (InputManager.Instance.getCrouch())
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            //Stop crouch
            if (!InputManager.Instance.getCrouch())
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }

        private void StateHandler()
        {
            if (isWallRun)
            {
                state = MovementState.wallrunning;
                desiredMoveSpeed = wallRunSpeed;
            }

            if (InputManager.Instance.getSlide())
            {
                state = MovementState.slide;

                if (OnSlope() && rb.velocity.y < 0.1f)
                {
                    desiredMoveSpeed = slideSpeed;
                }
                else
                {
                    desiredMoveSpeed = runSpeed;
                }
            }

            //Crouch
            if (InputManager.Instance.getCrouch())
            {
                state = MovementState.crouch;
                desiredMoveSpeed = crouchSpeed;
            }

            //Move
            else if (grounded && InputManager.Instance.getRun())
            {
                state = MovementState.run;
                desiredMoveSpeed = runSpeed;
            }
            else if (grounded)
            {
                state = MovementState.walk;
                desiredMoveSpeed = walkSpeed;
            }
            else 
            {
                state = MovementState.air;
            }

            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }

        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time/difference);

                if (OnSlope())
                {
                    float slopeAng = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngIncrease = 1 + (slopeAng / 90f);

                    time += Time.deltaTime * speedIncreaseMulti * slopeIncreaseMulti * slopeAngIncrease;
                }
                else
                {
                    time += Time.deltaTime * speedIncreaseMulti;
                }
                    
                yield return null;
            }

            moveSpeed = desiredMoveSpeed;
        }

        private void MovePlayer()
        {

            //Calculate movement
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            //On slope
            if (OnSlope())
            {
                rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20f, ForceMode.Force);
            
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            //Onground draag
            if (grounded)
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);
            }

            if (!isWallRun)
            {
                rb.useGravity = !OnSlope();
            }
        }

        private void SpeedControl()
        {
            //Limit speed on slope
            if (OnSlope() && !exitSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                //Limit speed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        private void Jump()
        {
            exitSlope = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyJump = true;
            exitSlope = false;
        }

        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight* 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        public void OnCollisionEnter(Collision col)
        {
            if (col.collider.CompareTag("Enemy"))
            {
                Debug.Log("Chat");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                desiredMoveSpeed = 0f;
                gameManage.isRunning = false;
            }
        }

        /*** Setter Getter for Horizontal ***/

        public float getHorizontalInput()
        {
            return horizontalInput;
        }

        public float getVerticalInput()
        {
            return verticalInput;
        }
    }
}

