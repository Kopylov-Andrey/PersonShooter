using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationAction : EntityAction
{
    [SerializeField] private Animator m_Animtor;

    [SerializeField] private string m_ActionAnimationName;

    [SerializeField] private float m_TimeDuration;

    private Timer m_Timer;
    private bool IsPlayAnimstion;

    public override void StartAction()
    {
        base.StartAction();

        m_Animtor.CrossFade(m_ActionAnimationName, m_TimeDuration);

        m_Timer = Timer.CreateTimer(m_TimeDuration, true);

        m_Timer.OnTick += OnTimerTick;
    }


    public override void EndAction()
    {
        base.EndAction();

        m_Timer.OnTick += OnTimerTick;
    }

    private void OnTimerTick()
    {
        if(m_Animtor.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName) == true)
        {
            IsPlayAnimstion = true;
        }
        
        if (IsPlayAnimstion == true)
        {
            if (m_Animtor.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName) == false)
            {
                IsPlayAnimstion = false;

                EndAction();
            }
        }

    }
}
