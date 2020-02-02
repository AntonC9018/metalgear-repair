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

    Patroling patroling;
    FieldOfView fov;

    public GameObject prefab;
    EnemyState enemyState = EnemyState.Patrol;

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        patroling = GetComponent<Patroling>();
    }


    void Update()
    {
        if (enemyState == EnemyState.Patrol)
        {
            patroling.Patrol();
            var visibleObjects = fov.GetVisible();
            if (fov.CheckPlayerInSight(visibleObjects))
            {
                enemyState = EnemyState.Attack;
            }
            fov.DrawSight();
        }
        else if (enemyState == EnemyState.Attack)
        {
            print("Attacking");
        }

    }
}
