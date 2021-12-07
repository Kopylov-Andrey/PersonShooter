using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBase<Player>
{
    [SerializeField]
    private UIPlayerNotic m_UIPlayerNotic;

    private int m_PersuersNumbers;

    public void StartPersuet()
    {
        m_PersuersNumbers++;
        m_UIPlayerNotic.Show();
    }

    public void StopPersuet()
    {
        m_PersuersNumbers--;
        if (m_PersuersNumbers == 0)
        {
            m_UIPlayerNotic.Hide();

        }
    }
}
