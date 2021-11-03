using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFirstAidKit : TriggerInteractionAction
{
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);

        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if(des != null)
        {
            des.HealFull();
        }

        Destroy(gameObject);
    }
}
