using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public CamMovement camMove;
    public InputController inputController;
    bool isZoomedin = true;

    // Update is called once per frame

    void Start ()
    {
        //isZoomedIn = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isZoomedin = !isZoomedin;

        }
    }
}
