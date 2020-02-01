using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float timeAlive = 0f;
    public int damage;
    public float speed;
    public string team;

    // Update is called once per frame
    void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;
        if (timeAlive > 5f)
            Destroy(gameObject);
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider collider) {
        Health enemyHealth = collider.GetComponent<Health>();
        if ((enemyHealth != null) && (collider.tag != team))
            enemyHealth.health -= damage;

        if (collider.tag != team)
        {
            print("Destroyed bullet");
            Destroy(gameObject);
        }
    }
}
