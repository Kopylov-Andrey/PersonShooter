using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSolider : Destructible
{
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }

}
