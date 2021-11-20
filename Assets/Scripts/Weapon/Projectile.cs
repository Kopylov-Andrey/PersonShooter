using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : Entity
{
    [SerializeField] private float m_Velocity;

    [SerializeField] private float m_Lifetime;

    [SerializeField] private int m_Damage;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefabStone;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefabMetal;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefabWood;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefabBlood;

    private float m_Timer;


    private void Update()
    {
        float stepLenght = Time.deltaTime * m_Velocity;

        Vector3 step = transform.forward * stepLenght;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward,out hit, stepLenght) == true)
        {
            if (hit.collider.isTrigger == false)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage, m_Parent);
                }

                OnProjetileLifeEnd(hit.collider, hit.point, hit.normal, hit);
            }

           
        }


        m_Timer += Time.deltaTime;

        if(m_Timer > m_Lifetime)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(step.x, step.y, step.z);
    }


    private void OnProjetileLifeEnd(Collider col , Vector3 pos, Vector3 normal, RaycastHit hit )
    {
        if(m_ImpactEffectPrefabStone != null)
        {
            if (hit.transform.tag == "StoneMaterial")
            {
                ImpactEffect impact = Instantiate(m_ImpactEffectPrefabStone, pos, Quaternion.LookRotation(normal));

                impact.transform.SetParent(col.transform);
            }
            if (hit.transform.tag == "FleshMaterial")   
            {
                ImpactEffect impact = Instantiate(m_ImpactEffectPrefabBlood, pos, Quaternion.LookRotation(normal));

                impact.transform.SetParent(col.transform);
            }
            if (hit.transform.tag == "WoodMaterial")
            {
                ImpactEffect impact = Instantiate(m_ImpactEffectPrefabWood, pos, Quaternion.LookRotation(normal));

                impact.transform.SetParent(col.transform);
            }

            if (hit.transform.tag == "MetalMaterial")
            {
                ImpactEffect impact = Instantiate(m_ImpactEffectPrefabMetal, pos, Quaternion.LookRotation(normal));

                impact.transform.SetParent(col.transform);
            }
        }
        Destroy(gameObject);
    }

    private Destructible m_Parent;

    public void SetParentShooter(Destructible parent)
    {
        m_Parent = parent;
    }

}

