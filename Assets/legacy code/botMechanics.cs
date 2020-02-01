// Patrol.cs

using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Net.Security;
using UnityEditor.Experimental.UIElements.GraphView;


public class botMechanics : MonoBehaviour
{

	
	public float RotationSpeed;
	public Transform[] points;
	private int destPoint = 0;
	
	public string[] actions;
	private int activity = 0;
	private bool isWaiting;

	public Transform[] lookAtTargets;
	private int currentLookAt = 0;
	
	private NavMeshAgent agent;
	private Transform CurrentTarget;
	/*
	public GameObject player;
	public FieldOfView fov;*/

	void Start ()
	{
		//fov = GetComponent<FieldOfView>();
		agent = GetComponent<NavMeshAgent>();
		//player = GameObject.Find("Player");
		StartCoroutine(GetNextAction());
	}

	IEnumerator GetNextAction()
	{
		
		if (actions.Length == 0)
			yield return null;
		else
		{

			if (actions[activity] == "next")
			{
				GotoNextPoint();
				////	print("working...");
			}
			else if (actions[activity] == "look")
			{
				////	print("looking");
				activity++;
				CurrentTarget = lookAtTargets[currentLookAt];
				isWaiting = true;
				yield return new WaitForSeconds(5);
				currentLookAt = (currentLookAt + 1) % lookAtTargets.Length;

			}

			activity = (activity + 1) % actions.Length;
			isWaiting = false;
		}
	}
	
	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;
		CurrentTarget = points[destPoint];
		////	print("targeting");
		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}


	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (!agent.pathPending && agent.remainingDistance < 1f && !isWaiting)
			StartCoroutine(GetNextAction());
		
		//make the bot always look at the target
		if (actions.Length != 0)
			LookAtTarget(CurrentTarget);
		//detects player
		//DetectPlayer();
	}

	void LookAtTarget(Transform targetLook)
	{	
		//Get the rotation delta
		var targetRotation = Quaternion.LookRotation(targetLook.transform.position - transform.position);
		targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0);
		
		//if (targetRotation.)
		// Smoothly rotate towards the target point.

		
		if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).y > RotationSpeed * Time.deltaTime)
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
		else
		{
			transform.rotation = targetRotation;
		}
		
	}
	/*
	void DetectPlayer()
	{
		var playerRayRotation = Quaternion.LookRotation(player.transform.position - transform.position).y ;
		print(NormalizeAngle((playerRayRotation - transform.rotation.y) * 360));
		if ((NormalizeAngle((playerRayRotation - transform.rotation.y) * 360) <= NormalizeAngle(fov.viewAngle / 2)) &&
		    (NormalizeAngle((playerRayRotation - transform.rotation.y) * 360) >= NormalizeAngle(-fov.viewAngle / 2)))
		{
			//inside
			Debug.DrawLine(transform.position, player.transform.position, Color.red);
		}
		else
		{
			//outside
			Debug.DrawLine(transform.position, player.transform.position, Color.blue);
		}
	}*/

	float NormalizeAngle(float angle)
	{
		if (angle < 0)
			return NormalizeAngle(angle + 360);
		else return angle % 360;
	}
}

