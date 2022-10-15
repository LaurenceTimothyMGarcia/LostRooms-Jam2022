using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class Sliding : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform playerObj;

        private Rigidbody rb;
        private PlayerController playCon;

        [Header("Sliding")]
        [SerializeField] private float maxSlideTime;
        [SerializeField] private float slideForce;
        [SerializeField] private float slideYScale;

        private float slideTimer;
        private float startYScale;

        private float horizontalInput;
        private float verticalInput;

        private bool isSliding;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            playCon = GetComponent<PlayerController>();

            startYScale = playerObj.localScale.y;
        }

        private void Update()
        {
            PlayerInput();

            if (InputManager.Instance.getSlide() && (horizontalInput != 0 || verticalInput != 0))
            {
                StartSlide();
            }

            if (!InputManager.Instance.getSlide() && playCon.isSliding)
            {
                StopSlide();
            }
        }

        private void FixedUpdate()
        {
            if (playCon.isSliding)
            {
                SlidingMovement();
            }
        }

        private void StartSlide()
        {
            playCon.isSliding = true;

            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            slideTimer = maxSlideTime;
        }

        private void SlidingMovement()
        {
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (!playCon.OnSlope() || rb.velocity.y > -0.1f)
            {
                rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

                slideTimer -= Time.deltaTime;
            }
            else
            {
                rb.AddForce(playCon.GetSlopeMoveDirection(inputDir) * slideForce, ForceMode.Force);
            }

            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

        private void StopSlide()
        {
            playCon.isSliding = false;

            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
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
        }
    }
}

