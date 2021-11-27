using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WheelAxle
{
    [SerializeField]
    private WheelCollider m_LeftWheelCollider;

    [SerializeField]
    private WheelCollider m_RightWheelCollider;

    [SerializeField]
    private Transform m_LeftWheelMesh;

    [SerializeField]
    private Transform m_RightWheelMesh;

    [SerializeField]
    private bool m_Motor;
    public bool Motor => m_Motor;

    [SerializeField]
    private bool m_Streering;
    public bool Streering => m_Streering;

    public void SetTorque(float torque)
    {
        if (m_Motor == false) return;

        m_LeftWheelCollider.motorTorque = torque;
        m_RightWheelCollider.motorTorque = torque;
    } 

    public void Break(float breakTorque)
    {
        m_LeftWheelCollider.brakeTorque = breakTorque;
        m_RightWheelCollider.brakeTorque = breakTorque;
    }

    public void SetStreerAngle(float angle)
    {
        if (m_Streering == false) return;

        m_LeftWheelCollider.steerAngle = angle;
        m_RightWheelCollider.steerAngle = angle;
        
    }

    public void UpdateMeshTransform()
    {
        UpdateWheelTransform(m_LeftWheelCollider, ref m_LeftWheelMesh);
        UpdateWheelTransform(m_RightWheelCollider, ref m_RightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, ref Transform wheelTransorm)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransorm.position = position;
        wheelTransorm.rotation = rotation;
    }
}


[RequireComponent(typeof(Rigidbody))]
public class WheeledVehicle : Vehicle
{
    [SerializeField]
    private WheelAxle[] m_WheelAxles;

    [SerializeField]
    private float m_MaxMotorTorque;

    [SerializeField]
    private float m_MaxSteerAngle;

    [SerializeField]
    private float m_BreakTorque;

    public override float LinearVelocity => m_Rigidbody.velocity.magnitude;


    private Rigidbody m_Rigidbody;

    protected override void Start()
    {
        base.Start();

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float targetMotor = m_MaxMotorTorque * TargetInputControl.z;
        float breakTorque = m_BreakTorque * TargetInputControl.y;
        float steering = m_MaxSteerAngle * TargetInputControl.x;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            if (breakTorque == 0 && LinearVelocity < m_MaxLinearVelocity)
            {
                m_WheelAxles[i].Break(0);
                m_WheelAxles[i].SetTorque(targetMotor);
            }
            if (LinearVelocity > m_MaxLinearVelocity)
            {
                m_WheelAxles[i].Break(breakTorque * 0.2f);
            }
            else
            {
                m_WheelAxles[i].Break(breakTorque);
            }



            m_WheelAxles[i].Break(breakTorque);




            m_WheelAxles[i].SetStreerAngle(steering);
            m_WheelAxles[i].UpdateMeshTransform();
        }
    }
}
