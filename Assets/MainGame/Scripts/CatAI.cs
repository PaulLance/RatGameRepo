using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CatAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navAgent;
    bool canChase = false;


    public void EnableChase()
    {
        canChase = true;
    }

   

    void Update()
    {
        if (canChase == false) { return; }
        if (target == null)
        {
            ThirdPersonUserControl tpuc = FindObjectOfType<ThirdPersonUserControl>();
            if (tpuc)
            {
                target = tpuc.transform;
            }
        }
        else
        {
            navAgent.SetDestination(target.position);
        }
        
    }
}
