using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement TargetCarecter;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig[] Rigs;
    [SerializeField] private UnityEngine.Animations.Rigging.MultiAimConstraint NeckMultiAimState;
    [SerializeField] private UnityEngine.Animations.Rigging.MultiAimConstraint NeckMultiAimCrouch;

    //
    [SerializeField] private float ChangeWeightLerpRate;

    private float targetWeight;
    private float targetWeightNeckState;
    private float targetWeightNeckCrouch;

    private void Update()
    {
        for (int i = 0; i < Rigs.Length; i++)
        {
            Rigs[i].weight = Mathf.MoveTowards(Rigs[i].weight, targetWeight, Time.deltaTime * ChangeWeightLerpRate);
        }

        if (TargetCarecter.IsAiming == true)
        {
            targetWeight = 1;
        }
        else
        {
            targetWeight = 0;
        }

        NeckMultiAimState.weight = Mathf.MoveTowards(NeckMultiAimState.weight, targetWeightNeckState, Time.deltaTime * ChangeWeightLerpRate);
        NeckMultiAimCrouch.weight = Mathf.MoveTowards(NeckMultiAimCrouch.weight, targetWeightNeckCrouch, Time.deltaTime * ChangeWeightLerpRate);

        if (TargetCarecter.IsCrouch == true)
        {
            targetWeightNeckState = 0;
            targetWeightNeckCrouch = 1;
        }
        else
        {
            targetWeightNeckState = 1;
            targetWeightNeckCrouch = 0;
        }
    }
}
