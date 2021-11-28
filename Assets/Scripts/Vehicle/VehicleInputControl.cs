using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInputControl : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonCamera m_Camera;

    [SerializeField]
    private Vehicle m_Vehicle;

    [SerializeField]
    private Vector3 m_CameraOffset;

    protected virtual void Start()
    {
        if (m_Camera != null)
        {
            m_Camera.IsRotateTarget = false;
            m_Camera.SetTargetOffset(m_CameraOffset);
        }
    }

    protected virtual void Update()
    {
        m_Vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));

        m_Camera.RotationCintrol = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void AssignCamera(ThirdPersonCamera camera)
    {
        m_Camera = camera;

        m_Camera.IsRotateTarget = false;

        m_Camera.SetTargetOffset(m_CameraOffset);
        m_Camera.SetTarget(m_Vehicle.transform);
    }

}

       
