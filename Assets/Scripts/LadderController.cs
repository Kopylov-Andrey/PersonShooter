using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : TriggerInteractionAction
{
    [SerializeField]
    private CharacterController m_CheracterController;
    [SerializeField]
    private float m_Speed;


    private bool m_IsEndOfStairs;
    public bool IsEndOfStairs => m_IsEndOfStairs;

    private Vector3 MoveDerection = Vector3.zero;

    private void Start()
    {
        
    }

    private void Update()
    {
        MoveDerection = new Vector3(0, Input.GetAxis("Vertical"), 0);

        MoveDerection = transform.TransformDirection(MoveDerection);

        MoveDerection *= m_Speed;

        m_CheracterController.Move(MoveDerection * Time.deltaTime);

    }
}
