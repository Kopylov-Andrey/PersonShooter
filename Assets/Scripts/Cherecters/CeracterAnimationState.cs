using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeracterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP_RATE = 10.0f;


    [SerializeField] private CharacterController TargetCharacterController;

    [SerializeField] private CheracterMovement TargetCharacterMovement;



    [SerializeField] private Animator TargetAnimator;

    private Vector3 InputControl;

    private void Update()
    {
        Vector3 movementSpeed =transform.InverseTransformDirection(TargetCharacterController.velocity);

        InputControl = Vector3.MoveTowards(InputControl, TargetCharacterMovement.TargetDirectionConrol, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

        TargetAnimator.SetFloat("Mormalize Movement X", InputControl.x);
        TargetAnimator.SetFloat("Mormalize Movement Z", InputControl.z);

        TargetAnimator.SetBool("Is Sprint", TargetCharacterMovement.IsSprint);
        TargetAnimator.SetBool("Is Crouch", TargetCharacterMovement.IsCrouch);
        TargetAnimator.SetBool("Is Aiming", TargetCharacterMovement.IsAiming);
        TargetAnimator.SetBool("Is Ground", TargetCharacterMovement.IsGrounded);


        if (TargetCharacterMovement.IsGrounded == false)
        {
            TargetAnimator.SetFloat("Jump", movementSpeed.y);

        }

        Vector3 groundSpeed = TargetCharacterController.velocity;
        groundSpeed.y = 0;
        TargetAnimator.SetFloat("Distance To Ground", TargetCharacterMovement.DistanceToGround);
        TargetAnimator.SetFloat("Ground Speed", groundSpeed.magnitude);
    }
}
