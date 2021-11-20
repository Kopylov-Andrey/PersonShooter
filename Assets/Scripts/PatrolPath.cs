using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField]
    private PatrolPathNode[] m_Nodes;

    private void Start()
    {
        UpdatepathNode();
    }

    [ContextMenu("Update Path Node")]
    private void UpdatepathNode()
    {
        m_Nodes = new PatrolPathNode[transform.childCount];

        for (int i = 0; i < m_Nodes.Length; i++)
        {
            m_Nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
        }
    }

    public PatrolPathNode GetRandomPathNode()
    {
        return m_Nodes[Random.Range(0, m_Nodes.Length)];
    }

    public PatrolPathNode GetNextNode(ref int index)
    {
        index = Mathf.Clamp(index, 0, m_Nodes.Length - 1);

        index++;

        if (index >= m_Nodes.Length)
        {
            index = 0;
        }

        return m_Nodes[index];
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_Nodes == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < m_Nodes.Length - 1 ; i++)
        {
            Gizmos.DrawLine(m_Nodes[i].transform.position + new Vector3(0, 0.5f, 0), m_Nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
        }

        Gizmos.DrawLine(m_Nodes[0].transform.position + new Vector3(0, 0.5f, 0), m_Nodes[m_Nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
    }


#endif

}
