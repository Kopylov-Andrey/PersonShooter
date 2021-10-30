using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;


    [SerializeField] private WeaponProperties m_TurretProperties;

    private float m_RefireTimer;

    public bool canFire => m_RefireTimer <= 0;

    //private SpaceShip m_Ship;

    #region Unity event
    private void Start()
    {
       // m_Ship = transform.root.GetComponent<SpaceShip>();
    }

    private void Update()
    {
        if (m_RefireTimer > 0)
            m_RefireTimer -= Time.deltaTime;
    }
    #endregion


    //Public API
    public void Fire()
    {
        if (m_TurretProperties == null) return;

        if (m_RefireTimer > 0) return;

       // if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
       //     return;


        //if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
        //    return;

        Projectile projectile = Instantiate(m_TurretProperties.Projectileprefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;

       // projectile.SetParentShooter(m_Ship);

        m_RefireTimer = m_TurretProperties.RateOfFire;


           
    }

    public void AssignLoadout(WeaponProperties props)
    {
        if (m_Mode != props.Mode) return;

        m_RefireTimer = 0;

        m_TurretProperties = props;
    }
}
