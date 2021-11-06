using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAnimatorParamatersName
{
    public string NormaliazeMovementX;
    public string NormaliazeMovementZ;
    public string Sprint;
    public string Crouch;
    public string Aiming;
    public string Ground;
    public string Jump;
    public string OnStairs;
    public string GroundSpeed;
    public string DistanceToGround;

}
[System.Serializable]
public class AnimationCrossFadeParameters
{
    public string Name;
    public float Duration;
}


public class CeracterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP_RATE = 10.0f;


    [SerializeField] private CharacterController TargetCharacterController;

    [SerializeField] private CheracterMovement TargetCharacterMovement;

    [SerializeField] [Space(5)] private CharacterAnimatorParamatersName AnimatorParameterNames;

    [SerializeField] private Animator TargetAnimator;

    [SerializeField]  [Header("Fades")] [Space(5)]
    private AnimationCrossFadeParameters FallFade;

    [SerializeField] private float MinDstanceToGroundByFall;

    [SerializeField] private AnimationCrossFadeParameters JumpIdleFade;

    [SerializeField] private AnimationCrossFadeParameters JumpMoveFade;

    private Vector3 InputControl;

    private void Update()
    {
        Vector3 movementSpeed =transform.InverseTransformDirection(TargetCharacterController.velocity);

        InputControl = Vector3.MoveTowards(InputControl, TargetCharacterMovement.TargetDirectionConrol, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

        TargetAnimator.SetFloat(AnimatorParameterNames.NormaliazeMovementX, InputControl.x);
        TargetAnimator.SetFloat(AnimatorParameterNames.NormaliazeMovementZ, InputControl.z);

        TargetAnimator.SetBool(AnimatorParameterNames.Sprint, TargetCharacterMovement.IsSprint);
        TargetAnimator.SetBool(AnimatorParameterNames.Crouch, TargetCharacterMovement.IsCrouch);
        TargetAnimator.SetBool(AnimatorParameterNames.Aiming, TargetCharacterMovement.IsAiming);
        TargetAnimator.SetBool(AnimatorParameterNames.Ground, TargetCharacterMovement.IsGrounded);


        Vector3 groundSpeed = TargetCharacterController.velocity;
        groundSpeed.y = 0;
        TargetAnimator.SetFloat(AnimatorParameterNames.GroundSpeed, groundSpeed.magnitude);


        if (TargetCharacterMovement.IsJump == true)
        {
            if (groundSpeed.magnitude <= 0.01f)
            {
                CrossFade(JumpIdleFade);
            }

            if (groundSpeed.magnitude > 0.01f)
            {
                CrossFade(JumpMoveFade);
            }
        }


        if (TargetCharacterMovement.IsGrounded == false)
        {
            TargetAnimator.SetFloat(AnimatorParameterNames.Jump, movementSpeed.y);

            if (movementSpeed.y < 0 && TargetCharacterMovement.DistanceToGround > MinDstanceToGroundByFall)
            {
                CrossFade(FallFade);
            }
        }

        TargetAnimator.SetFloat(AnimatorParameterNames.DistanceToGround, TargetCharacterMovement.DistanceToGround);
      }

    private void CrossFade(AnimationCrossFadeParameters parametrs)
    {
        TargetAnimator.CrossFade(parametrs.Name, parametrs.Duration);
    }
}
