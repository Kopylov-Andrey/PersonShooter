using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



/// <summary>
/// Уничтожаемый объект на сцене, то что может иметь хит поинты
/// </summary>
public class Destructible : Entity
{


    #region Properties    
    /// <summary>
    /// Объект игнорирует повреждения.
    /// </summary>
    [SerializeField]
    private bool m_Indestructible;
    public bool IsIndestructible => m_Indestructible;

    /// <summary>
    /// Стартовое кол-во хитпоинтов.
    /// </summary>
    [SerializeField]
    private int m_HitPoints;

    /// <summary>
    /// Текущие хитпоинты
    /// </summary>
    private int m_CurrentHitPoints;
    public int HitPoints => m_CurrentHitPoints;

    private bool m_IsDeath = false;

    #endregion



    #region Unity Events
    protected virtual void Start()
    {
        m_CurrentHitPoints = m_HitPoints;
    }

    #endregion



    #region Public API    
    /// <summary>
    /// Применение дамага к объекту.
    /// </summary>
    /// <param name="damage">Урон объекту</param>
    public void ApplyDamage(int damage)
    {
        if (m_Indestructible || m_IsDeath) return;


        m_CurrentHitPoints -= damage;


        if (m_CurrentHitPoints <= 0)
        {
            m_IsDeath = true;
            OnDeath();
          
        }

       
    }

    public void ApplyHeak(int heal)
    {
        m_CurrentHitPoints += heal;

        if (m_CurrentHitPoints > m_HitPoints)
            m_CurrentHitPoints = m_HitPoints;
    }

    public void HealFull()
    {
        m_CurrentHitPoints = m_HitPoints;
    }


    #endregion




    /// <summary>
    /// Переопределяющее событие уничтожения объектов
    /// </summary>
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
        m_EventOnDeath?.Invoke();
    }

    public static Destructible FindNearest(Vector3 position)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructbles)
        {
            float curDist = Vector3.Distance(dest.transform.position, position);

            if (curDist < minDist)
            {
                minDist = curDist;
                target = dest;
            }
        }

        return target;
    }

    public static Destructible FindNearestNonTeamMember(Destructible destructible)
    {
        float minDist = float.MaxValue;
        Destructible target = null;
        foreach (Destructible dest in m_AllDestructbles)
        {
            float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);

            if (curDist < minDist && destructible.TeamId != dest.TeamId)
            {
                minDist = curDist;
                target = dest;
            }
        }

        return target;

    }

    public static List<Destructible> GetAllTeamMember(int teamId)
    {
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructbles)
        {
            if (dest.TeamId == teamId)
            {
                teamDestructible.Add(dest);
            }
        }
        return teamDestructible;
    }

    public static List<Destructible> GetAllNonTeamMember(int teamId)
    {
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructbles)
        {
            if (dest.TeamId != teamId)
            {
                teamDestructible.Add(dest);
            }
        }
        return teamDestructible;
    }




    public static HashSet<Destructible> m_AllDestructbles;

    public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructbles; 

    protected virtual void OnEnable()
    {
        if (m_AllDestructbles == null)
        {
            m_AllDestructbles = new HashSet<Destructible>();

        }
        m_AllDestructbles.Add(this);
                       
    }
    protected virtual void OnDestroy()
    {
        m_AllDestructbles.Remove(this);
    }

    public const int TeamIdNeutral = 0;

    [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;


    [SerializeField]
    private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;

    #region Score

    [SerializeField] private int m_ScoreValue;

    public int  ScoreValue => m_ScoreValue;

    #endregion
}




