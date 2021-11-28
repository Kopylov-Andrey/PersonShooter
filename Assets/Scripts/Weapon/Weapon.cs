using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;


    [SerializeField] private WeaponProperties m_WeaponProperties;

    [SerializeField] private Transform m_FirePoint;

    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private ParticleSystem m_MuzzleParticleSystem;

    [SerializeField] private float m_PrimaryMaxEnergy;

    private float m_RefireTimer;
    public bool CanFire => m_RefireTimer <= 0 && EnergyIsRestored == false;

    public float PrimaryMaxEnergy => m_PrimaryMaxEnergy;
    public float PrimaryEnergy => m_PrimaryEnergy;

    private float m_PrimaryEnergy;

    private bool EnergyIsRestored;


    #region Unity event

    private Destructible m_Owner;

    private void Start()
    {
        m_PrimaryEnergy = m_PrimaryMaxEnergy;
        m_Owner = transform.root.GetComponent<Destructible>();
    }

    protected virtual void Update()
    {
        if (m_RefireTimer > 0)
            m_RefireTimer -= Time.deltaTime;

        UpdateEnergy();
    }
    #endregion

    private void UpdateEnergy()
    {
        m_PrimaryEnergy += (float)m_WeaponProperties.EnergyRegenPerSecond * Time.fixedDeltaTime;
        m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_PrimaryMaxEnergy);

        if (m_PrimaryEnergy >= m_WeaponProperties.EnergyAmountToStartFire)
        {
            EnergyIsRestored = false;
        }
    }

    private bool TryDrawEnergy(int count)
    {
        if (count == 0)
            return true;
        if (m_PrimaryEnergy >= count)
        {
            m_PrimaryEnergy -= count;
            return true;
        }

        EnergyIsRestored = true;

        return false;
    }


    //Public API
    public void Fire()
    {
        if (EnergyIsRestored == true) return;

        if (CanFire == false) return;
        
        if (m_WeaponProperties == null) return;

        if (m_RefireTimer > 0) return;

        if (TryDrawEnergy(m_WeaponProperties.EnergyUsage) == false) return;


        //if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
        //    return;

        Projectile projectile = Instantiate(m_WeaponProperties.Projectileprefab).GetComponent<Projectile>();
        projectile.transform.position = m_FirePoint.position;
        projectile.transform.forward = m_FirePoint.forward;

        projectile.SetParentShooter(m_Owner);

        m_RefireTimer = m_WeaponProperties.RateOfFire;


        m_MuzzleParticleSystem.time = 0;
        m_MuzzleParticleSystem.Play();

        //m_AudioSource.clip = m_WeaponProperties.LaunchSFX;
        m_AudioSource.PlayOneShot(m_WeaponProperties.LaunchSFX);
           
    }

    public void FirePointLookAt(Vector3 pos)
    {
        Vector3 offset = Random.insideUnitSphere * m_WeaponProperties.SpreadShotRange;
        if (m_WeaponProperties.SpraedShotDistanceFactor != 0)
            offset = offset * Vector3.Distance(m_FirePoint.position, pos) * m_WeaponProperties.SpraedShotDistanceFactor;
        m_FirePoint.LookAt(pos + offset);
    }

    public void AssignLoadout(WeaponProperties props)
    {
        if (m_Mode != props.Mode) return;

        m_RefireTimer = 0;

        m_WeaponProperties = props;
    }
}
