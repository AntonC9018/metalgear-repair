using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nozzle;
    public GameObject bullet;
    Ray mouseFollowRay;
    public Camera mainCamera;
    RaycastHit hit;
    public Light muzzleFlash;
    public float muzzleFlashTimer;
    public float heightDelta = 1f;
    public GameObject keepFiringAt;


    public void AimAndFirePlayer()
    {
        AimPlayer();
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void AimAndFireBot(GameObject target)
    {
        AimBot(target);
        Fire();
    }

    void AimBot(GameObject target)
    {
        transform.LookAt(target.transform.position);
    }

    void AimPlayer()
    {
        mouseFollowRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseFollowRay, out hit))
            if (hit.collider.tag == "Enemy" || hit.collider.tag == "Teammate")
                transform.LookAt(new Vector3(hit.point.x, hit.collider.transform.position.y, hit.point.z));
            else
                transform.LookAt(hit.point + Vector3.up * heightDelta);
    }
    // Update is called once per frame
    void Fire()
    {  
        Instantiate(bullet, nozzle.transform.position, nozzle.transform.rotation);
        StartCoroutine(MuzzleFlash());
    }

    private float timePassed = 0;
    public float fireperiod = 2;

    void Update()
    {
        if (keepFiringAt != null)
        {
            AimBot(keepFiringAt);
            if (timePassed > fireperiod)
            {
                timePassed = 0;
                Fire();
            }
            timePassed += Time.deltaTime;
        }
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.intensity = 3f;
        yield return new WaitForSeconds(muzzleFlashTimer);
        muzzleFlash.intensity = 0f;
    }
}
