using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAtTopOfLadder : MonoBehaviour
{
    [SerializeField]
    private EntityAnimationAction EndActionController;
    [SerializeField]
    private EntityAnimationAction StartActionController;
    [SerializeField]
    private Transform PersonPosition;
    [SerializeField]
    private Transform MahsPosition;

    public void OnTriggerEnter(Collider other)
    {

        StartActionController.StartAction();
        //EndActionController.EndAction();
    }

    public void NormalizationCharactersPosition()
    {
        Vector3 pos = MahsPosition.localPosition;

        PersonPosition.position = PersonPosition.position + pos;

        MahsPosition.localPosition = new  Vector3(0, 0, 0);
    }

}
