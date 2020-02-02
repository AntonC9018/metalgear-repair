using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Steering : MonoBehaviour
{

    public bool selected = true;
    NavMeshAgent agent;
    Gun gun;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gun = GetComponentInChildren<Gun>(); //?
    }

    public void BeSelected()
    {
        selected = true;
    }

    public void BeDeselected()
    {
        selected = false;
    }


    public bool moving = false;

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
