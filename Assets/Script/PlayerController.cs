using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class PlayerController : MonoBehaviour
    {
        //Movement
        [SerializeField] private float movementSpeed;
        [SerializeField] private float maxMoveSpeed;

        private float activeMoveSpeed;
        private float prevMoveSpeed;

        //Dash
        [SerializeField] private float dashSpeed;   //Speed of dash
        [SerializeField] private float dashTime;    //Dash last
        [SerializeField] private float dashCooldown;//Cooldown of dash
        [SerializeField] private float dashLength;  //Travel length of dash

        private float dashTimeCounter;              //Dash time counter
        private float dashCoolCounter;              //current cooldown
        private Vector3 tempVelocity;               //Speed before dash

        //Jump
        [SerializeField] private float jumpHeight;  //Jump velocity
        [SerializeField] private int jumpCount;     //extra jumps
        [SerializeField] private float fallGravScale;

        private int remainJumps;
        private bool isGrounded;


        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

