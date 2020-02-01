using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class charMovement : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent Agent;
    public Text detectStatusText;
    public float DistanceThreshold;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //GoToClick();
        NavmeshArrowMove();
    }

    void GoToClick()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Agent.destination = hit.point;
            }
        }
    }


    void NavmeshArrowMove()
    {
        Agent.destination = new Vector3(Agent.transform.position.x + Input.GetAxis("Horizontal"), Agent.transform.position.y, Agent.transform.position.z + Input.GetAxis("Vertical"));
    }

}
