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
        if (m_Indestructible) return;


        m_CurrentHitPoints -= damage;


        if (m_CurrentHitPoints <= 0) OnDeath();


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




