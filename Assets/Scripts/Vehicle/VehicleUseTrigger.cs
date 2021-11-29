using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUseTrigger : TriggerInteractionAction
{
    [SerializeField]
    private ActionUseVehicleProperties m_UseProperties;


    protected override void InitActionProperties()
    {
        m_Action.SetProperties(m_UseProperties);
    }

}
