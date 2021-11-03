using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class AIDrone : MonoBehaviour
{
    [SerializeField] private CubeArea m_MovementArea;
    [SerializeField] private float m_AngryDistance;

    private Drone m_Drone;

    private Vector3 m_MovementPosition;
    private Transform m_ShootTarget;

    private Transform m_Player;

    // Unity Events
    private void Start()
    {
        m_Drone = GetComponent<Drone>();
        m_Drone.EventOnDeath.AddListener(OnDroneDeath);

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        m_Drone.EventOnDeath.RemoveListener(OnDroneDeath);

    }

    private void Update()
    {
        UpdateAI();
    }

    // Handers
    private void OnDroneDeath()
    {
        enabled = false;
    }

    // AI

    private void UpdateAI()
    {
        //Update movement position
        if(transform.position == m_MovementPosition)
        {
            m_MovementPosition = m_MovementArea.GetRandomInsideZone();
        }

        if(Physics.Linecast(transform.position, m_MovementPosition) == true)
        {
            m_MovementPosition = m_MovementArea.GetRandomInsideZone();
        }
        //Finde Fire Position
        if (Vector3.Distance(transform.position, m_Player.position) <= m_AngryDistance)
        {
            m_ShootTarget = m_Player;
        }

        // Move
        m_Drone.MoveTo(m_MovementPosition);

        if (m_ShootTarget != null)
        {
            m_Drone.LookAt(m_ShootTarget.position);
        }
        else
        {
            m_Drone.LookAt(m_MovementPosition);
        }

        // Fire

        if (m_ShootTarget != null)
        {
            m_Drone.Fire(m_ShootTarget.position);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_AngryDistance);
    }
#endif
}
