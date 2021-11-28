using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingVehicleInputControl : VehicleInputControl
{
    [SerializeField]
    private CameraShooter m_CameraShooter;

    [SerializeField]
    private Transform m_AimPoint;

    protected override void Update()
    {
        base.Update();

        m_AimPoint.position = m_CameraShooter.Camera.transform.position +  m_CameraShooter.Camera.transform.forward * 30;

        if (Input.GetMouseButton(0))
        {
            m_CameraShooter.Shoot();
        }
    }
}
