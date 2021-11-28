using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUseVehicle :ActionInteract
{

    private void Start()
    {
        EventOnStart.AddListener(OnActionStarted);

        EventOnEnd.AddListener(OnActionEnded);
     
    }

    private void OnDestroy()
    {
        EventOnStart.RemoveListener(OnActionStarted);

        EventOnEnd.RemoveListener(OnActionEnded);
    }

    private void OnActionStarted()
    {
        Debug.Log("Start");
      
        IsCanEnd = true;

    }

    private void OnActionEnded()
    {
        Debug.Log("End");
    }
}
