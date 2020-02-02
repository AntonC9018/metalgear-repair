using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EnemyState
{
    Patrol,
    Attack
}

public class Enemy : MonoBehaviour
{
    List<GameObject> visiblePlayers;
    Patroling patroling;
    FieldOfView fov;
    float escapeRange = 10f;

    public GameObject prefab;
    EnemyState enemyState = EnemyState.Patrol;
    Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        patroling = GetComponent<Patroling>();
        gun = GetComponentsInChildren<Gun>()[0];
    }


    void Update()
    {
        print(enemyState.ToString());
        if (enemyState == EnemyState.Patrol)
        {
            patroling.Patrol();
            visiblePlayers = fov.GetVisible();
            if (fov.CheckPlayerInSight(visiblePlayers))
            {
                enemyState = EnemyState.Attack;

            }
            fov.DrawSight();
        }
        else if (enemyState == EnemyState.Attack)
        {
            patroling.Halt();
            var targets = FilterPlayers(visiblePlayers);

            if (targets.Count == 0) {
                enemyState = EnemyState.Patrol;
                patroling.Continue();
            }
            else {
                print("Attacking");

                transform.LookAt(targets[0].transform.position);
                gun.keepFiringAt = targets[0];

                visiblePlayers = fov.GetVisible();
            }
        }
    }

    List<GameObject> FilterPlayers(List<GameObject> players) {
        List<GameObject> actualPlayers = new List<GameObject>();
        foreach(GameObject player in players)
            if (player != null && player.tag == "Teammate" && (transform.position - player.transform.position).magnitude < escapeRange)
                actualPlayers.Add(player);

        return(actualPlayers);
    }
}
