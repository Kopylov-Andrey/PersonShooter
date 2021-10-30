using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShootRig : MonoBehaviour
{
    [SerializeField] private UnityEngine.Animations.Rigging.Rig SpreadRigs;
   

    
    [SerializeField] private float ChangeWeightLerpRate;

    private float targetWeight;
   

    private void Update()
    {
        SpreadRigs.weight = Mathf.MoveTowards(SpreadRigs.weight, targetWeight, Time.deltaTime * ChangeWeightLerpRate);
        
        if (SpreadRigs.weight == 1)
        {
            targetWeight = 0;
        }
        
       
    }

    public void Spread()
    {
        targetWeight = 1;
    }
}
