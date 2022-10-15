using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }


        //Initial Movement
        private bool moveForward;
        private bool moveBackward;
        private bool moveRight;
        private bool moveLeft;

        //Jump
        private bool jump;

        //Movement boosts
        private bool run;
        private bool dash;

        /* Setter Getter Methods */
        public void setMoveForward(bool forward)
        {
            moveForward = forward;
        }

        public void setMoveBackward(bool backward)
        {
            moveBackward = backward;
        }

        public void setMoveRight(bool right)
        {
            moveRight = right;
        }

        public void setMoveLeft(bool left)
        {
            moveLeft = left;
        }

        public void setJump(bool j)
        {
            jump = j;
        }

        public void setRun(bool r)
        {
            run = r;
        }

        public void setDash(bool d)
        {
            dash = d;
        }

        public bool getMoveForward()
        {
            return moveForward;
        }

        public bool getMoveBackward()
        {
            return moveBackward;
        }

        public bool getMoveRight()
        {
            return moveRight;
        }

        public bool getMoveLeft()
        {
            return moveLeft;
        }

        public bool getJump()
        {
            return jump;
        }

        public bool getRun()
        {
            return run;
        }

        public bool getDash()
        {
            return dash;
        }
    }
}

