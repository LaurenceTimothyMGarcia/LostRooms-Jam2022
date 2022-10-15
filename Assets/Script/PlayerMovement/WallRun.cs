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

        [Header("References")]
        [SerializeField] private Transform orientation;
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
            if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
            {
                //Start wall run


            }
        }

        private void StartWallRun()
        {

        }

        private void StopWallRun()
        {
            
        }
    }
}

