using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSolider : Destructible
{
    [SerializeField] private EntityAnimationAction ActionDeath;

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();


        ActionDeath.StartAction();
    }
}
