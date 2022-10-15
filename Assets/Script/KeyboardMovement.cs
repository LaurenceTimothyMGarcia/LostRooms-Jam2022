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

            //Dash
            if (Input.GetKey(KeyCode.LeftShift))
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

