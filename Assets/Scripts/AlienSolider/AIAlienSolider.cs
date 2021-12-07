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
        PursuetTarget,
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
    private float m_AimingDistance;

    [SerializeField]
    private float m_DetectedDistance;

    [SerializeField]
    private int m_PatrolPathNodeIndex = 0;

    private NavMeshPath m_NavMeshPath;
    private PatrolPathNode currentPathNode;

    private GameObject potentionalTarget;
    private Transform pursueTarget;
    private Vector3 seekTarget;
    private bool IsPlayerDetected;

    // Unity Event

    private void Start()
    {
        potentionalTarget = Player.Instance.gameObject;

        m_CharacterMovement.UpdatePosition = false;
        m_NavMeshPath = new NavMeshPath();

        StartBehaviour(m_AIBehavior);

        m_AlienSolider.OnGetDamage += OnGetDamage;
        m_AlienSolider.EventOnDeath.AddListener(OnDeath);
    }

    private void OnDestroy()
    {
        m_AlienSolider.OnGetDamage -= OnGetDamage;
        m_AlienSolider.EventOnDeath.RemoveListener(OnDeath);
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    // Handler
    private void OnGetDamage(Destructible other)
    {
        if (other.TeamId != m_AlienSolider.TeamId)
        {
            ActionAssignTargetAllTeamMember(other.transform);
        }
    }

    private void OnDeath()
    {
        SendPlayerStopPersute();
    }


    // AI

    private void UpdateAI()
    {
        ActionUpdateTarget();

        if (m_AIBehavior == AIBehavior.Idle)
            return;

        if (m_AIBehavior == AIBehavior.PursuetTarget)
        {
            m_Agent.CalculatePath(pursueTarget.position, m_NavMeshPath);
            m_Agent.SetPath(m_NavMeshPath);
            
            if (Vector3.Distance(transform.position, pursueTarget.position) <= m_AimingDistance)
            {
                m_CharacterMovement.Aiming();
                if (Vector3.Angle(pursueTarget.position - transform.position, transform.forward)< 10 )
                {
                    m_Agent.isStopped = true;
                }
               
                m_AlienSolider.Fire(pursueTarget.position + new Vector3(0, 1, 0));
            }
            else
            {
                m_CharacterMovement.UnAiming();
            }

        }

        if (m_AIBehavior == AIBehavior.SeekTarget)
        {
            SendPlayerStartPersute();
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
            SendPlayerStopPersute();

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

        if (m_ColliderViewer.IsObjectVisible(potentionalTarget) == true || Vector3.Distance(transform.position, potentionalTarget.transform.position) < m_DetectedDistance)
        {
            SendPlayerStartPersute();

            pursueTarget = potentionalTarget.transform;
            ActionAssignTargetAllTeamMember(pursueTarget);
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
        if (m_AlienSolider.IsDeath == true) return;

        if (state == AIBehavior.Idle)
        {
            m_Agent.isStopped = true;

            m_CharacterMovement.UnAiming();
           
        }

        if (state == AIBehavior.PatrolRandom)
        {
            m_Agent.isStopped = false;
            m_CharacterMovement.UnAiming();
            SetDistinationByPathNode(m_PatrolPath.GetRandomPathNode());
        }

        if (state == AIBehavior.CirclePatrole)
        {
            
            m_Agent.isStopped = false;
            m_CharacterMovement.UnAiming();
            SetDistinationByPathNode(m_PatrolPath.GetNextNode(ref m_PatrolPathNodeIndex));
        }
        

        if (state == AIBehavior.PursuetTarget)
        {
            m_Agent.isStopped = false;
        }
        if (state == AIBehavior.SeekTarget)
        {
            m_Agent.isStopped = false;
            m_CharacterMovement.UnAiming();
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

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(m_AlienSolider.TeamId);

        foreach (Destructible dest in team)
        {
            AIAlienSolider ai = dest.transform.root.GetComponent<AIAlienSolider>();

            if(ai != null && ai.enabled == true)
            {
                ai.SetPursueTarget(other);
                ai.StartBehaviour(AIBehavior.PursuetTarget);
            }

        }
    }


    public void SetPursueTarget(Transform target)
    {
        pursueTarget = target;
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

    private void SendPlayerStartPersute()
    {
        if (IsPlayerDetected == false)
        {
            Player.Instance.StartPersuet();
            IsPlayerDetected = true;
        }
    }
    private void SendPlayerStopPersute()
    {
        if (IsPlayerDetected == true)
        {
            Player.Instance.StopPersuet();
            IsPlayerDetected = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_DetectedDistance);
    }
#endif

}
