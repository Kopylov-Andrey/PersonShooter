using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAlienSolider : MonoBehaviour
{
   public enum AIBehavior
    {
        Null,
        Idle,
        PatrolRandom,
        CirclePatrole,
        PursueTarget,
        SeekTarget
    }

    [SerializeField]
    private AIBehavior m_AIBehavior;

    [SerializeField]
    private AlienSolider m_AlienSolider;

    [SerializeField]
    private CharacterMovement m_CharacterMovement;

    [SerializeField]
    private NavMeshAgent m_Agent;

    [SerializeField]
    private PatrolPath m_PatrolPath;

    [SerializeField]
    private ColliderViewer m_ColliderViewer;

    [SerializeField]
    private int m_PatrolPathNodeIndex = 0;

    private NavMeshPath m_NavMeshPath;
    private PatrolPathNode currentPathNode;

    private GameObject potentionalTarget;
    private Transform pursueTarget;
    private Vector3 seekTarget;

    // Unity Event

    private void Start()
    {
        potentionalTarget = Destructible.FindNearestNonTeamMember(m_AlienSolider)?.gameObject;

        m_CharacterMovement.UpdatePosition = false;
        m_NavMeshPath = new NavMeshPath();

        StartBehaviour(m_AIBehavior);
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    // AI

    private void UpdateAI()
    {
        ActionUpdateTarget();

        if (m_AIBehavior == AIBehavior.Idle)
            return;

        if (m_AIBehavior == AIBehavior.PursueTarget)
        {
            m_Agent.CalculatePath(pursueTarget.position, m_NavMeshPath);
            m_Agent.SetPath(m_NavMeshPath);
        }

        if (m_AIBehavior == AIBehavior.SeekTarget)
        {
            m_Agent.CalculatePath(seekTarget, m_NavMeshPath);
            m_Agent.SetPath(m_NavMeshPath);

            if (AgentReachedDistination() == true)
            {
                StartBehaviour(AIBehavior.PatrolRandom);
            }
        }

        if (m_AIBehavior == AIBehavior.PatrolRandom)
        {
            if (AgentReachedDistination() == true) 
            {
                StartCoroutine(SetBehavioutOnTime(AIBehavior.Idle, currentPathNode.IdleTime));
            }
        }

        if (m_AIBehavior == AIBehavior.CirclePatrole)
        {
            if (AgentReachedDistination() == true)
            {
                StartCoroutine(SetBehavioutOnTime(AIBehavior.Idle, currentPathNode.IdleTime));
            }
        }
    }

    // Actions

    private void ActionUpdateTarget()
    {
        if (potentionalTarget == null) return;

        if (m_ColliderViewer.IsObjectVisible(potentionalTarget) == true)
        {
            pursueTarget = potentionalTarget.transform;
            StartBehaviour(AIBehavior.PursueTarget);
        }
        else
        {
            if (pursueTarget != null)
            {
                seekTarget = pursueTarget.position;
                pursueTarget = null;
                StartBehaviour(AIBehavior.SeekTarget);
            }
        }
      
    }

  

    // Behaviour

    private void StartBehaviour(AIBehavior state)
    {
        if (state == AIBehavior.Idle)
        {
            m_Agent.isStopped = true;
           
        }

        if (state == AIBehavior.PatrolRandom)
        {
            m_Agent.isStopped = false;
            SetDistinationByPathNode(m_PatrolPath.GetRandomPathNode());
        }

        if (state == AIBehavior.CirclePatrole)
        {
            m_Agent.isStopped = false;
            SetDistinationByPathNode(m_PatrolPath.GetNextNode(ref m_PatrolPathNodeIndex));
        }
        m_AIBehavior = state;
    }
    IEnumerator SetBehavioutOnTime(AIBehavior state, float second)
    {
        AIBehavior previous = m_AIBehavior;
        m_AIBehavior = state;
        StartBehaviour(m_AIBehavior);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }

    //Private Method

    private void SetDistinationByPathNode(PatrolPathNode node)
    {
        currentPathNode = node;

        m_Agent.CalculatePath(node.transform.position, m_NavMeshPath);
        m_Agent.SetPath(m_NavMeshPath);
    }

    private bool AgentReachedDistination()
    {
        if (m_Agent.pathPending == false)
        {
            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
            {
                if (m_Agent.hasPath == false || m_Agent.velocity.sqrMagnitude == 0.0f)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    private void SyncAgentAndCharacterMovement()
    {
        m_Agent.speed = m_CharacterMovement.CurrentSpeed;


        float factor = m_Agent.velocity.magnitude / m_Agent.speed;
        m_CharacterMovement.TargetDirectionConrol = transform.InverseTransformDirection(m_Agent.velocity.normalized) * factor;
    }
}
