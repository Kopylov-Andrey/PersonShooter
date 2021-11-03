using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHitPointSlider : MonoBehaviour
{
    [SerializeField] private Destructible m_Destructible;

    [SerializeField] private Slider m_Slider;
  

    private void Update()
    {
        m_Slider.value = m_Destructible.HitPoints;

      
    }
}
