using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private float Sencetive;
    [SerializeField] private float CangeOffsetRate;
    [SerializeField] private float RotateTargetRate;

    [Header("Rotation Limit")]
    [SerializeField] private float MaxLimitY;
    [SerializeField] private float MinLimitY;
    
    [Header("Distance Camera")]
    [SerializeField] private float Distance;
    [SerializeField] private float MinDistance;
    [SerializeField] private float DistanceLerpRate;
    [SerializeField] private float DistanceOffsetFromCollisionHit;
    

    [HideInInspector] public bool IsRotateTarget;

    [HideInInspector] public Vector2 RotationCintrol;

    private float DeltaRotationX;
    private float DeltaRotationY;

    private float CurrentDistance;

    private Vector3 TargetOffset;
    private Vector3 DefaultOffset;

    private void Start()
    {
        DefaultOffset = Offset;
        TargetOffset = Offset;
    }

    private void Update()
    {
        // Calculate rotation and translation
        DeltaRotationX += RotationCintrol.x * Sencetive;
        DeltaRotationY += RotationCintrol.y * -Sencetive;

        DeltaRotationY = ClampAngle(DeltaRotationY, MinLimitY, MaxLimitY);

        Offset = Vector3.MoveTowards(Offset, TargetOffset, Time.deltaTime * CangeOffsetRate);// интерпол€ци€ камеры

        Quaternion finalRotation = Quaternion.Euler(DeltaRotationY, DeltaRotationX, 0);
        Vector3 finalPosition = Target.position - (finalRotation * Vector3.forward * Distance);
        finalPosition = AddLocalOffset(finalPosition);


        // Calculate current distance
        float targetDistance = Distance;

        RaycastHit hit;

        Debug.DrawLine(Target.position + new Vector3(0, Offset.y, 0), finalPosition, Color.red);

        if (Physics.Linecast(Target.position + new Vector3(0,Offset.y, 0), finalPosition, out hit) == true)
        {
            float distanceToHit = Vector3.Distance(Target.position + new Vector3(0, Offset.y, 0), hit.point);
            if (hit.transform != Target)
            {
                if (distanceToHit < Distance)
                {
                    targetDistance = distanceToHit - DistanceOffsetFromCollisionHit;
                }
            }
         
        }

        CurrentDistance = Mathf.MoveTowards(CurrentDistance, targetDistance, Time.deltaTime * DistanceLerpRate);

        CurrentDistance = Mathf.Clamp(CurrentDistance, MinDistance, Distance);

        // Correct camera position 
        finalPosition = Target.position - (finalRotation * Vector3.forward * CurrentDistance);



        // Apply transform
        transform.rotation = finalRotation;
        transform.position = finalPosition;
        transform.position = AddLocalOffset(transform.position);



        // Rotation target
        if (IsRotateTarget == true)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Target.rotation = Quaternion.RotateTowards(Target.rotation, targetRotation, Time.deltaTime * RotateTargetRate);
        }
    }

    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result += new Vector3(0, Offset.y, 0);
        result += transform.right * Offset.x;
        result += transform.forward * Offset.z;

        return result;
    }



    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        
        return Mathf.Clamp(angle, min, max); 
    }

    public void SetTargetOffset(Vector3 offset)
    {
        TargetOffset = offset;
      
    }
    public void SetDefaultOffset()
    {
        TargetOffset = DefaultOffset;

    }

}
