using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSolider : Destructible
{
    [SerializeField]
    private Weapon m_Weapon;

    [SerializeField]
    private SpreadShootRig m_SpreadShootRig;

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }

    public void Fire(Vector3 target)
    {
        if (m_Weapon.CanFire == false) return;

        m_Weapon.FirePointLookAt(target);
        m_Weapon.Fire();
        m_SpreadShootRig.Spread();
    }

}
