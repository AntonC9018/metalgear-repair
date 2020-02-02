using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public int numRaycastsEachSide = 10;
    public float angle = 45;
    public float range = 10;
    Vector3 headOffset;

    void Setup()
    {
        headOffset = GetComponent<Patroling>().headOffset;
    }

    RaycastHit RaycastFieldOfVision(float angle, List<GameObject> hits)
    {
        var dir = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        Ray ray = new Ray();
        ray.origin = transform.position + headOffset;
        ray.direction = dir;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var obj = hit.transform.gameObject;
            var diff = obj.transform.position - (transform.position + headOffset);
            var dist = diff.sqrMagnitude;

            if (dist < range * range)
            {
                hits.Add(obj);
            }
        }
        return hit;
    }

    public List<GameObject> GetVisible()
    {

        List<GameObject> result = new List<GameObject>();

        RaycastFieldOfVision(0, result);

        for (int i = 1; i < numRaycastsEachSide; i++)
        {
            RaycastFieldOfVision(angle / (float)i, result);
            RaycastFieldOfVision(-angle / (float)i, result);
        }

        return result;
    }


    public bool CheckPlayerInSight(List<GameObject> visibleObjects)
    {
        foreach (var obj in visibleObjects)
        {
            if (obj.tag == "Teammate")
            {
                return true;
            }
        }
        return false;
    }

    public void DrawSight()
    {
        {
            var offset = Quaternion.AngleAxis(45, transform.up) * transform.forward * range;
            var pos = offset + transform.position + headOffset;
            //Instantiate(prefab, pos, Quaternion.identity);
        }

        {
            var offset = Quaternion.AngleAxis(-45, transform.up) * transform.forward * range;
            var pos = offset + transform.position + headOffset;
            //Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
