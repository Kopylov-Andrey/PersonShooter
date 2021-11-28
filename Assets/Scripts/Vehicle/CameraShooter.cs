using UnityEngine;

public class CameraShooter : MonoBehaviour
{
    [SerializeField] private Weapon m_Weapon;

    [SerializeField] private Camera m_Camera;
    public Camera Camera => m_Camera;

    [SerializeField] private RectTransform m_ImageSigh;



    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(m_ImageSigh.position);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            m_Weapon.FirePointLookAt(hit.point);
        }
        else
        {
            m_Weapon.FirePointLookAt(m_Camera.transform.position + ray.direction * 1000);
        }
        if (m_Weapon.CanFire == true)
        {
            m_Weapon.Fire();
           
        }
    }
}
