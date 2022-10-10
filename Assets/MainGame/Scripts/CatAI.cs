using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CatAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navAgent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
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
