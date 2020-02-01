using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFocus : MonoBehaviour
{
	public float camDistance;
	public bool TopDownOrAngled;
	public GameObject[] players;
	private Vector3 target;
	private void Awake()
	{
		
		players = GameObject.FindGameObjectsWithTag("Player");
	}

	// Update is called once per frame
	void LateUpdate ()
	{
	
		target = GetCenterPoint();

		if (TopDownOrAngled)
			transform.LookAt(target);
		else transform.position = players[0].transform.position + Vector3.up * camDistance;
	}

	Vector3 GetCenterPoint()
	{
		
		if (players.Length == 1)
			return players[0].transform.position;
		
		var bounds = new Bounds(players[0].transform.position, Vector3.zero);
		for (int i = 0; i < players.Length; i++)
		{
			bounds.Encapsulate(players[i].transform.position);
		}
		return bounds.center;
	}
}
