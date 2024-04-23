using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;


    public override void Update()
    {
        JoystickMove();
    }
    private void JoystickMove()
    {
        Vector3 movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        //Debug.Log(joystick.Horizontal +"------");
        if (movementDirection.magnitude > 0.1f)
        {
            //transform.position += movementDirection * Time.deltaTime * moveSpeed;
            rb.velocity = movementDirection * moveSpeed;


            //float targetAngle = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg + 90f;
            //Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            //transform.right = targetRotation * Vector3.forward;
            transform.forward = movementDirection;

            ChangeAnim("Run");
        }
        if(movementDirection.magnitude < 0.1f)
        {
            rb.velocity = Vector3.zero;
            ChangeAnim("Idle");
        }
    }


}