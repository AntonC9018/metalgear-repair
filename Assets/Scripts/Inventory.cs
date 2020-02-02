using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public int componentCount = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider cog) {
        if (cog.tag == "Cog") {
            componentCount++;
            Destroy(cog.gameObject);
        }
    }
}
