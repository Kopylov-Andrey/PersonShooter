using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverterCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform CameraLook;
    [SerializeField] private Vector3 LookOffset;

    [SerializeField] private float TopAngleLimit;
    [SerializeField] private float BottomAngleLimit;

    void Update()
    {
        Vector3 Angles = new Vector3(0, 0, 0);

        Angles.z = Camera.eulerAngles.x;

        if (Angles.z >= TopAngleLimit || Angles.z <= BottomAngleLimit)
        {
            transform.LookAt(CameraLook.position + LookOffset);

            Angles.x = transform.eulerAngles.x;
            Angles.y = transform.eulerAngles.y;

            transform.eulerAngles = Angles;
        }
    }
    
}
