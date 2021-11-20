using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField]
    private float m_ViewingAngle;

    [SerializeField]
    private float m_ViewingDistance;

    [SerializeField]
    private float m_ViewHeight;


    // public API

    public bool IsObjectVisible(GameObject target)
    {
        ColliderViewPoints viewPoint = target.GetComponent<ColliderViewPoints>();

        if (viewPoint == false) return false;

        return viewPoint.IsVisibleFromPoint(transform.position + new Vector3(0, m_ViewHeight, 0), transform.forward, m_ViewingAngle, m_ViewingDistance);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, m_ViewHeight, 0), transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, m_ViewingAngle, 0, m_ViewingDistance, 1);
    }
#endif
}
