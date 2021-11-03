using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]

public class TriggerInteractionAction : MonoBehaviour
{
    [SerializeField] private InteractType m_InteractType;

    [SerializeField] private int m_InteractAmount;

    [SerializeField] private ActionInteractProperties m_ActionProperties;

    [SerializeField] private UnityEvent m_EventOnInteract;

    public UnityEvent EventOnInteract => m_EventOnInteract;

    protected ActionInteract m_Action;

    private GameObject m_Owner;


    protected virtual void InitActionProperties()
    {
        m_Action.SetProperties(m_ActionProperties);
    }


    protected virtual void OnStartAction(GameObject owner) { }
    protected virtual void OnEndAction(GameObject owner) { }



    private void OnTriggerEnter(Collider other)
    {

        if (m_InteractAmount == 0 /*|| m_InteractAmount != -1*/) return;

        EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

        if(actionCollector != null)
        {
           m_Action = GetActionInteract(actionCollector);

            if (m_Action != null)
            {
                InitActionProperties();
                m_Action.IsCanStart = true;
                m_Action.EventOnStart.AddListener(ActionStarted);
                m_Action.EventOnEnd.AddListener(ActionEnded);
                m_Owner = other.gameObject;
              
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (m_InteractAmount == 0 /*|| m_InteractAmount != -1*/) return;


        EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

        if (actionCollector != null)
        {
            m_Action = GetActionInteract(actionCollector);

            if (m_Action != null)
            {
                m_Action.IsCanStart = false;

                m_Action.EventOnStart.RemoveListener(ActionStarted);
                m_Action.EventOnEnd.RemoveListener(ActionEnded);
            }
        }
    }



    private void ActionStarted() 
    {
        OnStartAction(m_Owner);

       

    }

    private void ActionEnded() 
    {
        m_Action.IsCanStart = false;
        m_Action.EventOnStart.RemoveListener(ActionStarted);
        m_Action.EventOnEnd.RemoveListener(ActionEnded);

        m_EventOnInteract?.Invoke();

        m_InteractAmount--;

        OnEndAction(m_Owner);
    }

    private ActionInteract GetActionInteract(EntityActionCollector entityActionCollector)
    {

        List<ActionInteract> actions = entityActionCollector.GetActionList<ActionInteract>();

        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].Type == m_InteractType)
            {
                return actions[i];
            }
        }

        return null;
    }
}
