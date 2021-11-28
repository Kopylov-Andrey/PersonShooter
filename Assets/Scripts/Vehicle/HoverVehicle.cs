using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverVehicle : Vehicle
{
    [SerializeField]
    private float m_ThrustForward;

    [SerializeField]
    private float m_ThrustTorque;

    [SerializeField]
    private float m_DragLinear;

    [SerializeField]
    private float m_DragAngular;

    [SerializeField]
    private float m_HoverHeight;

    [SerializeField]
    private float m_HoverForce;

    [SerializeField]
    private float m_MaxLinearSpeed;

    [SerializeField]
    private Transform[] m_HoverJets;

    private Rigidbody m_Rigidbody;


    protected override void Start()
    {
        base.Start();

        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        ComputeForces();

    }

    private bool m_IsGround;

    private void ComputeForces()
    {
        m_IsGround = false;


        for (int i = 0; i < m_HoverJets.Length; i++)
        {
            if(ApplyJetForce(m_HoverJets[i]) == true)
            {
                m_IsGround = true;
            }
        }

        if (m_IsGround == true)
        {
            m_Rigidbody.AddRelativeForce(Vector3.forward * m_ThrustForward * TargetInputControl.z);
            m_Rigidbody.AddRelativeTorque(Vector3.up * m_ThrustTorque * TargetInputControl.x);
        }


        // Linear drag
        {
            float DragCoeff = m_ThrustForward / m_MaxLinearSpeed;
            Vector3 dragForce = m_Rigidbody.velocity * -DragCoeff;

            if (m_IsGround == true)
            {
                m_Rigidbody.AddForce(dragForce, ForceMode.Acceleration);
            }
        }

        // Angular drag
        {
            Vector3 dragForce = m_Rigidbody.angularVelocity * m_DragAngular;

            m_Rigidbody.AddTorque(dragForce, ForceMode.Acceleration);
        }

    }

    public bool ApplyJetForce(Transform transform)
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_HoverHeight) == true )
        {
            float s = (m_HoverHeight - hit.distance) / m_HoverHeight;

            Vector3 force = (s * m_HoverForce) * hit.normal;

            m_Rigidbody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);
            return true;
        }
        return false;
    }

}
