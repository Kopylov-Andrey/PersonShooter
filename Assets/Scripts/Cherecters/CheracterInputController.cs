using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheracterInputController: MonoBehaviour
{

    [SerializeField] private CheracterMovement TargetCheracterMovement;

    [SerializeField] private EntityActionCollector TargetActionCollector;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            List<EntityContextAction> actionList = TargetActionCollector.GetActionList<EntityContextAction>();

            for(int i = 0; i < actionList.Count; i++)
            {
                actionList[i].StartAction();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if(TargetCheracterMovement.IsAiming == true)
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