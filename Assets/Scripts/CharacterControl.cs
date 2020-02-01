using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    CharacterController charControl;
    float heightDelta;
    RaycastHit hit;
    Ray mouseFollowRay;
    public float speed;
    public GameObject playerObject;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        charControl.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * speed);
        
        mouseFollowRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseFollowRay, out hit))
            playerObject.transform.LookAt(new Vector3(hit.point.x, playerObject.transform.position.y, hit.point.z));
    }


}
