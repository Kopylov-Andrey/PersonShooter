using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform MainMash;
    [SerializeField] private Transform DronTransform;

    [Header("View")]
    [SerializeField] private GameObject[] MeshComponents;
    [SerializeField] private Renderer[] MeshRenderers;
    [SerializeField] private Material[] DeadMaterials;

    [Header("Moving")]
    [SerializeField] private float HoverAmplitude;
    [SerializeField] private float HoverSpeed;
    [SerializeField] private float Speed;

    private bool IsEndPoint = true;
    private Vector3 endPointPatrol;
    private Vector3 StartPointPatrol;


    private void Update()
    {
        MainMash.position += new Vector3(0, Mathf.Sin(Time.time * HoverAmplitude) * HoverSpeed * Time.deltaTime, 0);

        PointPatrol();
    }

    

    private void PointPatrol()
    {
        if (IsEndPoint)
        {
            endPointPatrol = new Vector3(Random.Range(DronTransform.position.x - 25, DronTransform.position.x + 25), DronTransform.position.y, Random.Range(DronTransform.position.z -25, DronTransform.position.z + 25));
            IsEndPoint = false;
            StartPointPatrol = DronTransform.position;
        }

        if (DronTransform.position != endPointPatrol)
        {
            DronTransform.position = new Vector3(Mathf.Lerp(DronTransform.position.x, endPointPatrol.x, Time.deltaTime * Speed), DronTransform.position.y, Mathf.Lerp(DronTransform.position.z, endPointPatrol.z, Time.deltaTime * Speed));
           
        }

        if ((int)DronTransform.position.x == (int)endPointPatrol.x && (int)DronTransform.position.z == (int)endPointPatrol.z)
        {
            
            IsEndPoint = true;
        }
    }

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();

        enabled = false;

        for (int i = 0; i < MeshComponents.Length; i++)
        {
            if (MeshComponents[i].GetComponent<Rigidbody>() == null)
            {
                MeshComponents[i].AddComponent<Rigidbody>();
            }
        }

        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].material = DeadMaterials[i];
        }

    }
}
