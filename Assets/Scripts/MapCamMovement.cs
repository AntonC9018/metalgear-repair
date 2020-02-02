using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamMovement : MonoBehaviour
{
    public CharacterController mapCamControl;
    public MapToggle mapToggle;
    // Update is called once per frame
    void Update()
    {
        if (!mapToggle.isZoomedin) {
            mapCamControl.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        }
    }
}
