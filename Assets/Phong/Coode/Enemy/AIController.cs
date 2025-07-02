using UnityEngine;
using UnityEngine.Collection.Generic;
using UnityEngine.Collections;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float moveSpeed = 2;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;
    /////////////////////////////////
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1;
    public int edgeIterations = 4;
    public float edgeDistanceThreshold = 0.5f;

    public Tranform[] waypoints;
    int m_currentWaypointIndex = 0;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_targetPosition;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRanger = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_currentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestionation(waypoints[m_currentWaypointIndex].position);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
        // Logic for when the AI catches the player
        Debug.Log("Player caught!");
    }
    void Move(float speed)
    {
        if (m_CaughtPlayer)
        {
            return;
        }

        if (m_PlayerInRanger)
        {
            m_targetPosition = m_PlayerPosition;
            navMeshAgent.speed = speedRun;
        }
        else
        {
            m_targetPosition = waypoints[m_currentWaypointIndex].position;
            navMeshAgent.speed = speedWalk;
        }

        navMeshAgent.SetDestination(m_targetPosition);
    }
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestionation(player);
        if (Vector3.Distance(transform.position, player) < 0.5)
        {
            if (m_WaitTime > 0)
            {
                m_PlayerNear = false;
                Move();
            }
        }

    }
}
