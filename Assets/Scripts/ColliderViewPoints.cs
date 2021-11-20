using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderViewPoints : MonoBehaviour
{
    private enum ColliderType
    {
        Character
    }

    [SerializeField]
    private ColliderType m_ColliderType;

    [SerializeField]
    private Collider m_Collider;

    private Vector3[] m_Points;


    private void Start()
    {
        if (m_ColliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }

    }


    private void Update()
    {
        if (m_ColliderType == ColliderType.Character)
        {
            CalcPointForCharacterController(m_Collider as CharacterController);
        }
    }

    //Public API
    public bool IsVisibleFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
    {
        for (int i = 0; i < m_Points.Length; i++)
        {
            float angle = Vector3.Angle(m_Points[i] - point, eyeDir);
            float dist = Vector3.Distance(m_Points[i], point);

            if (angle <= viewAngle * 0.5f && dist <= viewDistance)
            {
                RaycastHit hit;

                Debug.DrawLine(point, m_Points[i], Color.blue);
                if (Physics.Raycast(point, (m_Points[i] - point).normalized, out hit, viewDistance * 2) == true)
                {
                    if (hit.collider == m_Collider)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }



    [ContextMenu("UpdateViewPoint")]
    private void UpdateViewPoints()
    {
        if (m_Collider == null) return;

        m_Points = null;

        if (m_ColliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }


    }


    private void UpdatePointsForCharacterController()
    {
        if (m_Points == null)
        {
            m_Points = new Vector3[4];
        }

        CharacterController collider = m_Collider as CharacterController;

        CalcPointForCharacterController(collider);


    }

    private void CalcPointForCharacterController(CharacterController collider)
    {
        m_Points[0] = collider.transform.position + collider.center + collider.transform.up * collider.height * 0.3f;
        m_Points[1] = collider.transform.position + collider.center - collider.transform.up * collider.height * 0.3f;
        m_Points[2] = collider.transform.position + collider.center + collider.transform.right * collider.radius * 0.4f;
        m_Points[3] = collider.transform.position + collider.center - collider.transform.right * collider.radius * 0.4f;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_Points == null) return;
       

        Gizmos.color = Color.cyan;
        for (int i = 0; i < m_Points.Length; i++)
        {
            Gizmos.DrawSphere(m_Points[i], 0.1f);
        }
    }
#endif



}
