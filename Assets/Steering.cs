using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Steering : MonoBehaviour
{

    public bool selected = true;
    NavMeshAgent agent;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void BeSelected()
    {
        selected = true;
    }

    public void BeDeselected()
    {
        selected = false;
    }

    // Update is called once per frame
    public void MoveTo(Vector3 vec)
    {
        print("Setting target");
        agent.destination = vec;
    }
}
