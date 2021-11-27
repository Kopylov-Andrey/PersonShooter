using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : Destructible
{

    [SerializeField]
    protected float m_MaxLinearVelocity;


    public virtual float LinearVelocity => 0;

    public float NormalizedLinearVelocity
    {
        get
        {
            if (Mathf.Approximately(0,LinearVelocity ) == true)  return 0;

            return Mathf.Clamp01(LinearVelocity / m_MaxLinearVelocity);
        }
    }

    protected Vector3 TargetInputControl;

    public void SetTargetControl(Vector3 control)
    {
        TargetInputControl = control.normalized;
    }

    [Header("Engine Sound")]
    [SerializeField]
    private AudioSource m_EngineSFX;

    [SerializeField]
    private float m_EngineSFXModifer;

    protected virtual void Update()
    {
        UpdateEngineSFX();
    }

    private void UpdateEngineSFX()
    {
        if (m_EngineSFX != null)
        {
            m_EngineSFX.pitch = 1.0f + m_EngineSFXModifer * NormalizedLinearVelocity;
            m_EngineSFX.volume = 0.5f + NormalizedLinearVelocity;
        }
    }


}
