using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtHighlight : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, .2f);
	}
}
