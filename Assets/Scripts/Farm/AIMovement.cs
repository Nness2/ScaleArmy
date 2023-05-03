using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public float walkingSpeed = 1.5f;
    public float waitingTime = 10.0f;
    public int numWaypoints = 5;
    public float minDistance = 2.0f;
    public float maxDistance = 4.0f;

    //private int currentWaypoint = 0;
    public float waitTimer = 0.0f;
    private NavMeshAgent navAgent;
    private Transform[] waypoints;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = walkingSpeed;
        GenerateWaypoints();
        //SetRandomDestination();
        waitTimer = Random.Range(5, waitingTime);
    }

    void Update()
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
            {
                if (waitTimer >= waitingTime)
                {
                    GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Run);
                    waitTimer = 0.0f;
                    SetRandomDestination();
                }
                else
                {
                    waitTimer += Time.deltaTime;
                    GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
                }
            }
        }
    }

    void GenerateWaypoints()
    {
        waitTimer = Random.Range(5, waitingTime);

        waypoints = new Transform[numWaypoints];
        for (int i = 0; i < numWaypoints; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas);
            waypoints[i] = new GameObject("Waypoint " + i).transform;
            waypoints[i].position = hit.position;
        }
    }

    void SetRandomDestination()
    {
        int randomIndex = Random.Range(0, waypoints.Length);
        navAgent.SetDestination(waypoints[randomIndex].position);
    }

    public void StopMoving()
    {
        waypoints = new Transform[0];
    }
}