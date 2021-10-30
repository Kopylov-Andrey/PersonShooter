using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayerShooter : MonoBehaviour
{
    [SerializeField] private CheracterMovement m_CharacterMovement;

    [SerializeField] private Weapon m_Weapon;

    [SerializeField] private SpreadShootRig m_SpreadShootRig;

    [SerializeField] private Camera m_Camera;

    [SerializeField] private RectTransform m_ImageSigh;


    //[SerializeField] private Transform m_FirePoint;

    public void Shoot()
    {
        RaycastHit hit;

        Ray ray = m_Camera.ScreenPointToRay(m_ImageSigh.position);

        if (Physics.Raycast(ray,out hit ,1000) == true)
        {
            m_Weapon.FirePointLookAt(hit.point);
        }
        if (m_Weapon.CanFire == true)
        {
            m_Weapon.Fire();
            m_SpreadShootRig.Spread();
        }
        
    }
}
