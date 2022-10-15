using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class WallRun : MonoBehaviour
    {
        [Header("Wallrunning")]
        [SerializeField] private LayerMask whatIsWall;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float wallRunForce;
        [SerializeField] private float wallJumpUpForce;
        [SerializeField] private float wallJumpSideForce;
        [SerializeField] private float wallClimbSpeed;
        [SerializeField] private float maxWallRunTime;

        private float wallRunTimer;

        //Input
        private float horizontalInput;
        private float verticalInput;

        [Header("Detection")]
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float minJumpHeight;

        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

        [Header("Exit")]
        [SerializeField] float exitWallTime;

        private bool exitWall;
        private float exitWallTimer;

        [Header("Gravity")]
        [SerializeField] private bool useGravity;
        [SerializeField] private float gravityCounterForce;


        [Header("References")]
        [SerializeField] private Transform orientation;
        [SerializeField] private PlayerCam cam;
        private PlayerController playCon;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            playCon = GetComponent<PlayerController>();
        }

        private void Update()
        {
            CheckForWall();
            StateMachine();
        }

        private void FixedUpdate()
        {
            if (playCon.isWallRun)
            {
                WallRunningMovement();
            }
        }

        private void CheckForWall()
        {
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        }

        private bool AboveGround()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
        }

        private void StateMachine()
        {
            horizontalInput = playCon.getHorizontalInput();
            verticalInput = playCon.getVerticalInput();

            //Wall Running state
            if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitWall)
            {
                //Start wall run
                if (!playCon.isWallRun)
                {
                    StartWallRun();
                }

                if (wallRunTimer > 0)
                {
                    wallRunTimer -= Time.deltaTime;
                }

                if (wallRunTimer <= 0 && playCon.isWallRun)
                {
                    exitWall = true;
                    exitWallTimer = exitWallTime;
                }

                if (InputManager.Instance.getWallJump())
                {
                    WallJump();
                }
            }
            else if (exitWall)
            {
                if (playCon.isWallRun)
                {
                    StopWallRun();
                }

                if (exitWallTimer > 0)
                {
                    exitWallTimer -= Time.deltaTime;
                }

                if (exitWallTimer <= 0)
                {
                    exitWall = false;
                }
            }

            else
            {
                if (playCon.isWallRun)
                {
                    StopWallRun();
                }
            }
        }

        private void StartWallRun()
        {
            playCon.isWallRun = true;

            wallRunTimer = maxWallRunTime;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            cam.DoFOV(90f);
            //if (wallLeft) cam.DoTilt(-5f);
            //if (wallRight) cam.DoTilt(5f);
        }

        private void WallRunningMovement()
        {
            rb.useGravity = useGravity;

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            {
                wallForward = -wallForward;
            }

            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

            if (InputManager.Instance.getUpwardRun())
            {
                rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
            }
            if (InputManager.Instance.getDownwardRun())
            {
                rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
            }

            if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            {
                rb.AddForce(-wallNormal * 100, ForceMode.Force);
            }
                
            if (useGravity)
            {
                rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
            }

        }

        private void StopWallRun()
        {
            playCon.isWallRun = false;

            cam.DoFOV(80f);
            cam.DoTilt(0f);
        }

        private void WallJump()
        {
            exitWall = true;
            exitWallTimer = exitWallTime;

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 forceToApp = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(forceToApp, ForceMode.Impulse);
        }
    }
}

