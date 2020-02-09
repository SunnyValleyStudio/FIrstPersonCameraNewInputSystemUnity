using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float jumpHeight = 5f;
    [SerializeField]
    private float gravity = 20;

    private Vector2 input;
    private bool jumping = false;

    private Animator animator;

    private Vector3 movementDirtection = Vector3.zero;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(controller.isGrounded)
            jumping = true;
    }

    private void FixedUpdate()
    {
        var localDIrection = new Vector3(input.x, 0, input.y) * movementSpeed;
        var worldSpaceDirection = transform.TransformDirection(localDIrection);
        if (controller.isGrounded)
        {
            animator.SetFloat("velocity", input.magnitude);
            if (input.magnitude < 0.2f)
            {
                animator.SetFloat("velocity", 0);
            }
            if (jumping)
            {
                jumping = false;
                worldSpaceDirection.y = jumpHeight;
                if (localDIrection.magnitude > 0)
                {
                    animator.SetFloat("velocity", 0);
                }
            }
            else
            {
                worldSpaceDirection.y = 0;
            }
            movementDirtection = worldSpaceDirection;
        }
        movementDirtection.y -= gravity * Time.deltaTime;

        controller.Move(movementDirtection * Time.deltaTime);
    }
}
