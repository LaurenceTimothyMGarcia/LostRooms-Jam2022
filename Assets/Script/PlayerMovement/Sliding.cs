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
            
        }

        private void StartSlide()
        {

        }

        private void SlidingMovement()
        {

        }

        private void StopSlide()
        {

        }

    }
}

