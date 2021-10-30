using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponMode
{
    Primary,
    Secondary
}

[CreateAssetMenu]
public sealed  class WeaponProperties : ScriptableObject
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;



    [SerializeField] private Projectile m_Projectileprefab;
    public Projectile Projectileprefab => m_Projectileprefab;



    [SerializeField] private float m_RateOfFire;
    public float RateOfFire => m_RateOfFire;



    [SerializeField] private int m_EnergyUsage;
    public int EnergyUsage => m_EnergyUsage;



    [SerializeField] private AudioClip m_LaunchSFX;
    public AudioClip LaunchSFX => m_LaunchSFX;
}

