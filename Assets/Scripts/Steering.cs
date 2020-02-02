using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Steering : MonoBehaviour
{

    public bool selected = true;
    NavMeshAgent agent;
    Gun gun;
    Renderer rend;

    public void Start()
    {
        rend = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        gun = GetComponentInChildren<Gun>(); //?
    }

    public void BeSelected()
    {
        selected = true;
        rend.material.SetFloat("_Outline", 0.2f);
    }

    public void BeDeselected()
    {
        selected = false;
        rend.material.SetFloat("_Outline", 0.0f);
    }


    public void Attack(GameObject enemy)
    {
        MoveTo(enemy.transform.position);
        gun.keepFiringAt = enemy;
    }

    // Update is called once per frame
    public void MoveTo(Vector3 vec)
    {
        agent.destination = vec;
    }
}
