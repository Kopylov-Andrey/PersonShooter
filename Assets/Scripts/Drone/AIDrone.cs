using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class AIDrone : MonoBehaviour
{
    [SerializeField] private CubeArea m_MovementArea;
    [SerializeField] private ColliderViewer m_ColliderViewer;

    private Drone m_Drone;

    private Vector3 m_MovementPosition;
    private Transform m_ShootTarget;

    

    // Unity Events
    private void Start()
    {
        m_Drone = GetComponent<Drone>();
        m_Drone.EventOnDeath.AddListener(OnDroneDeath);

        m_Drone.OnGetDamage += OnGetDamage;
    }

    private void OnDestroy()
    {
        m_Drone.EventOnDeath.RemoveListener(OnDroneDeath);

        m_Drone.OnGetDamage-= OnGetDamage;

    }

    private void Update()
    {
        UpdateAI();
    }

    // Handers
    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetAllTeamMember(other.transform);
    }
    private void OnDroneDeath()
    {
        enabled = false;
    }

    // AI

    private void UpdateAI()
    {
        ActionFindeShootTarget();

        ActionMove();
        
        ActionFire();
    }

    // Action
    private void ActionFindeShootTarget()
    {
        Transform potentionalTarget = FindShootTarget();
        if (potentionalTarget != null)
        {
            ActionAssignTargetAllTeamMember(potentionalTarget);
        }
    }
    private void ActionMove()
    {
        if (transform.position == m_MovementPosition)
        {
            m_MovementPosition = m_MovementArea.GetRandomInsideZone();
        }

        if (Physics.Linecast(transform.position, m_MovementPosition) == true)
        {
            m_MovementPosition = m_MovementArea.GetRandomInsideZone();
        }

        m_Drone.MoveTo(m_MovementPosition);

        if (m_ShootTarget != null)
        {
            m_Drone.LookAt(m_ShootTarget.position);
        }
        else
        {
            m_Drone.LookAt(m_MovementPosition);
        }

    }
    
    private void ActionFire()
    {
        if (m_ShootTarget != null)
        {
            //if (m_ColliderViewer.IsObjectVisible(target.gameObject) == true)
                m_Drone.Fire(m_ShootTarget.position);
        }
    }

    // Method

    public void SetShootTarget(Transform target)
    {
        m_ShootTarget = target;
    }

    private Transform FindShootTarget()
    {
        List<Destructible> targets = Destructible.GetAllNonTeamMember(m_Drone.TeamId);

        for(int i = 0; i < targets.Count; i++)
        {
           
            if (m_ColliderViewer.IsObjectVisible(targets[i].gameObject) == true)
            {
                return targets[i].transform;
            }
        }

        return null;
    }

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(m_Drone.TeamId);

        foreach (Destructible dest in team)
        {
            AIDrone ai = dest.transform.root.GetComponent<AIDrone>();

            if (ai != null && ai.enabled == true)
            {
                ai.SetShootTarget(other);
              
            }

        }
    }


}
