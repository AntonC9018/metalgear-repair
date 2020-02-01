using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 camPosDelta;
    Vector3 movementAxisInput;
    public float movementMultiplier;
    
    public float camToMouseMultiplier;
    public Transform playerTransform;

    Vector3 finalCamOffset;


    void Start()
    {
        transform.localPosition = camPosDelta;
    }

    // Update is called once per frame
    void Update()
    {
        movementAxisInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        finalCamOffset = camPosDelta;

        if (Input.GetMouseButton(1))
            finalCamOffset += new Vector3 ((Input.mousePosition.x - Screen.width/2)/Screen.width, 0f, (Input.mousePosition.y - Screen.height/2)/Screen.height) * camToMouseMultiplier;
        else
            finalCamOffset += movementAxisInput * movementMultiplier;
        
        transform.localPosition = finalCamOffset;
    }


}
