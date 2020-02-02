using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    CharacterController charControl;
    
    public float characterSpeed;
    public GameObject playerObject;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        print("called");
        charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void MoveCharacter()
    {
        var ds = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * characterSpeed;

        charControl.Move(ds);

        Ray mouseFollowRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseFollowRay, out hit))
        {
            var hitPoint = new Vector3(hit.point.x, playerObject.transform.position.y, hit.point.z);
            playerObject.transform.LookAt(hitPoint);
        }
            
    }


}
