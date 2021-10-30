using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheracterMovementController : MonoBehaviour
{

    [SerializeField] private CheracterMovement TargetCheracterMovement;

    [SerializeField] private ThirdPersonCamera TargetCamera;

    [SerializeField] private PayerShooter TargetShooter;

    [SerializeField] private Vector3 AimingOffset;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        TargetCheracterMovement.TargetDirectionConrol = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        TargetCamera.RotationCintrol = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (TargetCheracterMovement.TargetDirectionConrol != Vector3.zero || TargetCheracterMovement.IsAiming == true)
        {
            TargetCamera.IsRotateTarget = true;
        }
        else
        {
            TargetCamera.IsRotateTarget = false;
        }

        if (Input.GetMouseButton(0))
        {
            TargetShooter.Shoot();
        }


        if (Input.GetMouseButtonDown(1))
        {
            
            TargetCheracterMovement.Aiming();
            TargetCamera.SetTargetOffset(AimingOffset);
        }

        if (Input.GetMouseButtonUp(1))
        {
            TargetCheracterMovement.UnAiming();
            TargetCamera.SetDefaultOffset();
        }

        if (Input.GetButtonDown("Jump"))
            TargetCheracterMovement.Jump();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TargetCheracterMovement.Crouch();
        }


        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            TargetCheracterMovement.UnCrouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TargetCheracterMovement.Sprint();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            TargetCheracterMovement.UnSprint();
        }
    }
}
