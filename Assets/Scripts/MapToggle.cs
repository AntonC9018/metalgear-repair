using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToggle : MonoBehaviour
{
    // Start is called before the first frame update
    //public CamMovement camMove;
    public InputController inputController;
    public bool isZoomedin = true;
    public Camera playerCamera;
    public Camera mapCamera;

    // Update is called once per frame

    void Start ()
    {
        //isZoomedIn = true;
        playerCamera.enabled = isZoomedin;
        mapCamera.enabled = !isZoomedin;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isZoomedin = !isZoomedin;
            playerCamera.enabled = isZoomedin;
            mapCamera.enabled = !isZoomedin;
        }
    }
}
