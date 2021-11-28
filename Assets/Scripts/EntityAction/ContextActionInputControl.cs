using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextActionInputControl : MonoBehaviour
{

    [SerializeField] private EntityActionCollector TargetActionCollector;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<EntityContextAction> actionList = TargetActionCollector.GetActionList<EntityContextAction>();

            for (int i = 0; i < actionList.Count; i++)
            {
                actionList[i].StartAction();
                actionList[i].EndAction();
            }
        }
    
    }
}
