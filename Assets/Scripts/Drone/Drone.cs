using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform m_MainMash;
    [SerializeField] private Transform DronTransform;
    [SerializeField] private Weapon[] m_Turrets;

    [Header("View")]
    [SerializeField] private GameObject[] MeshComponents;
    [SerializeField] private Renderer[] MeshRenderers;
    [SerializeField] private Material[] DeadMaterials;

    [Header("Movement")]
    [SerializeField] private float HoverAmplitude;
    [SerializeField] private float HoverSpeed;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float RotationLerpFactor;


    //private bool IsEndPoint = true;
    //private Vector3 endPointPatrol;

    public Transform MainMesh => m_MainMash;


    private void Update()
    {
        Hover();

        //PointPatrol();
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

    private void Hover()
    {
        m_MainMash.position += new Vector3(0, Mathf.Sin(Time.time * HoverAmplitude) * HoverSpeed * Time.deltaTime, 0);
    }


    //private void PointPatrol()
    //{
    //    if (IsEndPoint)
    //    {
    //        endPointPatrol = new Vector3(Random.Range(DronTransform.position.x - 25, DronTransform.position.x + 25), DronTransform.position.y, Random.Range(DronTransform.position.z -25, DronTransform.position.z + 25));
    //        IsEndPoint = false;
            
    //    }

    //    if (DronTransform.position != endPointPatrol)
    //    {
    //        DronTransform.position = new Vector3(Mathf.Lerp(DronTransform.position.x, endPointPatrol.x, Time.deltaTime * MovementSpeed), DronTransform.position.y, Mathf.Lerp(DronTransform.position.z, endPointPatrol.z, Time.deltaTime * MovementSpeed));
           
    //    }

    //    if ((int)DronTransform.position.x == (int)endPointPatrol.x && (int)DronTransform.position.z == (int)endPointPatrol.z)
    //    {
            
    //        IsEndPoint = true;
    //    }
    //}

   
    // Public API
    public void LookAt(Vector3 target)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), Time.deltaTime * RotationLerpFactor);
    }

    public void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MovementSpeed);
    }

    public void Fire(Vector3 target)
    {
        for (int i = 0; i < m_Turrets.Length; i++)
        {
            m_Turrets[i].FirePointLookAt(target);
            m_Turrets[i].Fire();
        }
    }
}
