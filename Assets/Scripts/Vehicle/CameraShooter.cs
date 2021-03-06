using UnityEngine;

public class CameraShooter : MonoBehaviour
{
    [SerializeField] private Weapon m_Weapon;

    [SerializeField] private Camera m_Camera;
    public Camera Camera => m_Camera;

    [SerializeField] private RectTransform m_ImageSigh;

    // проблем выстрела либо в камере либо в прицеле, попробовать стрелять в прицел а не в направление камеры

    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(m_ImageSigh.position);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            m_Weapon.FirePointLookAt(hit.point);
            //Debug.DrawRay(m_Camera.transform.position, ray.direction, Color.green);
            Debug.Log(hit.point);
        }
        else
        {
            m_Weapon.FirePointLookAt(m_Camera.transform.position + ray.direction * 1000);
            //Debug.DrawRay( , m_Camera.transform.position + ray.direction , Color.green);
        }
        if (m_Weapon.CanFire == true)
        {
            m_Weapon.Fire();
           
        }
    }
}
