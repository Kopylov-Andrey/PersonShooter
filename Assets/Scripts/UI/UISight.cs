using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISight : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharectrMovement;

    [SerializeField] private Image m_ImageSight;

    private void Update()
    {
        m_ImageSight.enabled = m_CharectrMovement.IsAiming;
    }
}
