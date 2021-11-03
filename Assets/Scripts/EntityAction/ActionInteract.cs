using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    PickupItem,
    EnteringCode

}

[System.Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform m_InteractTransform;

    public Transform InteractTransform => m_InteractTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] private Transform m_Owner;
    [SerializeField] private InteractType m_Type;

    public InteractType Type => m_Type;

    private new ActionInteractProperties Properties;

    public override void SetProperties(EntityActionProperties prop)
    {
        Properties = prop as ActionInteractProperties;
    }

    public override void StartAction()
    {
        if (IsCanStart == false) return;

        base.StartAction();

        m_Owner.position = Properties.InteractTransform.position;
    }
}
