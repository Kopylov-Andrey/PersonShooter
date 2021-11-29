using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityContextAction : EntityAnimationAction
{
    public bool IsCanStart;

    public bool IsCanEnd;


    public override void StartAction()
    {
        if (IsCanStart == false) return;
        base.StartAction();
    }
    public override void EndAction()
    {
        if (IsCanEnd == false) return;

        IsCanEnd = false;

        //base.StartAction();
        base.EndAction();
    }
}
