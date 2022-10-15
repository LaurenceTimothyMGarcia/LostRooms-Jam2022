using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInput
{
    public class KeyboardMovement : MonoBehaviour
    {
        void Update()
        {

            //Base Movement
            if (Input.GetKey(KeyCode.W))
            {
                InputManager.Instance.setMoveForward(true);
            }
            else
            {
                InputManager.Instance.setMoveForward(false);
            }

            if (Input.GetKey(KeyCode.S))
            {
                InputManager.Instance.setMoveBackward(true);
            }
            else
            {
                InputManager.Instance.setMoveBackward(false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                InputManager.Instance.setMoveLeft(true);
            }
            else
            {
                InputManager.Instance.setMoveLeft(false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                InputManager.Instance.setMoveRight(true);
            }
            else
            {
                InputManager.Instance.setMoveRight(false);
            }

            //Jump
            if (Input.GetKey(KeyCode.Space))
            {
                InputManager.Instance.setJump(true);
            }
            else
            {
                InputManager.Instance.setJump(false);
            }

            //Crouch
            if (Input.GetKey(KeyCode.LeftControl))
            {
                InputManager.Instance.setCrouch(true);
            }
            else
            {
                InputManager.Instance.setCrouch(false);
            }

            //Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                InputManager.Instance.setRun(true);
            }
            else
            {
                InputManager.Instance.setRun(false);
            }

            //Dash
            if (Input.GetKey(KeyCode.Q))
            {
                InputManager.Instance.setDash(true);
            }
            else
            {
                InputManager.Instance.setDash(false);
            }
        }
    }
}
