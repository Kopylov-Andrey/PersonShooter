using UnityEngine;
using UnityEngine.Events;

public abstract class EntityActionProperties
{

}

public abstract class EntityAction : MonoBehaviour
{
    [SerializeField] private UnityEvent m_EventOnStart;

    [SerializeField] private UnityEvent m_EventOnEnd;

    public UnityEvent EventOnStart => m_EventOnStart;
    public UnityEvent EventOnEnd => m_EventOnEnd;


    private EntityActionProperties m_Properties;
    public EntityActionProperties Properties => m_Properties;

    private bool m_isStarted;

    public virtual void StartAction()
    {
        if (m_isStarted == true) return;


        m_isStarted = true;
        m_EventOnStart?.Invoke();
    }

    public virtual void EndAction()
    {
        m_isStarted = false;

        m_EventOnEnd?.Invoke();
    }

    public virtual void SetProperties(EntityActionProperties prop)
    {
        m_Properties = prop;
    }
}
