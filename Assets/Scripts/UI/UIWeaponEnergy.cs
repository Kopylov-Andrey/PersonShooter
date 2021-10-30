using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponEnergy : MonoBehaviour
{
    [SerializeField] private Weapon m_TargetWeapon;

    [SerializeField] private Slider m_Slider;

    [SerializeField] private Image[] m_Image;

    private void Start()
    {
        m_Slider.maxValue = m_TargetWeapon.PrimaryMaxEnergy;
        m_Slider.value = m_Slider.maxValue;
    }

    private void Update()
    {
        m_Slider.value = m_TargetWeapon.PrimaryEnergy;

        SetActiveImages(m_TargetWeapon.PrimaryEnergy != m_TargetWeapon.PrimaryMaxEnergy);
    }

    private void SetActiveImages(bool active)
    {
        for (int i = 0; i < m_Image.Length; i++)
        {
            m_Image[i].enabled = active;
        }

    }
}
