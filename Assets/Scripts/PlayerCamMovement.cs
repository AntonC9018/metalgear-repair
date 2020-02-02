using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 camPosDelta;
    Vector3 movementAxisInput;
    public float movementMultiplier;
    
    public float camToMouseMultiplier;

    Vector3 finalCamOffset;
    


    void Start()
    {
        transform.localPosition = camPosDelta;
    }

    // Update is called once per frame
    public void FollowPlayer()
    {
        movementAxisInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        finalCamOffset = camPosDelta;

        if (Input.GetMouseButton(1))
        {
            var x = (Input.mousePosition.x - Screen.width / 2) / Screen.width;
            var z = (Input.mousePosition.y - Screen.height / 2) / Screen.height;
            finalCamOffset += new Vector3(x, 0f, z) * camToMouseMultiplier;
        }
            
        //else
            //finalCamOffset += movementAxisInput * movementMultiplier;
        
        transform.localPosition = finalCamOffset;
    }

}
