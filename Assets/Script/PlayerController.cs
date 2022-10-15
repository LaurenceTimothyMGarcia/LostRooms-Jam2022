using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float groundDrag;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMulti;

        private bool readyJump;

        [Header("GroundCheck")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask whatIsGround;
        private bool grounded;

        [SerializeField] private Transform orientation;

        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDir;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyJump = true;
        }

        // Update is called once per frame
        void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            PlayerInput();
            SpeedControl();

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
        }

        private void MovePlayer()
        {

            //Calculate movement
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            //Onground draag
            if (grounded)
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);
            }

        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit speed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyJump = true;
        }
    }
}

