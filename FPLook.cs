using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensititiy = 5f;
    [SerializeField]
    private float headPitchAngleUpLimit = -20f, headPitchAngleDownLimit = 70f;

    private Quaternion bodyOriginOrientation, headOriginOrientation;
    private float currentYaw = 0f, currentPitch = 0f;

    private float horizontalInput, verticalInput;

    [SerializeField]
    private Transform head;

    // Start is called before the first frame update
    void Start()
    {
        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        bodyOriginOrientation = transform.localRotation;
        headOriginOrientation = head.localRotation;
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput *= Time.deltaTime * mouseSensititiy;
        verticalInput *= Time.deltaTime * mouseSensititiy;

        currentYaw += horizontalInput;
        currentPitch += verticalInput;
        currentPitch = Mathf.Clamp(currentPitch, headPitchAngleUpLimit, headPitchAngleDownLimit);
    }

    private void FixedUpdate()
    {
        var bodyRotation = Quaternion.AngleAxis(currentYaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(-currentPitch, Vector3.right);

        transform.localRotation = bodyRotation * bodyOriginOrientation;
        head.localRotation = headRotation * headOriginOrientation;
    }
}
