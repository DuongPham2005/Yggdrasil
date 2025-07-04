using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float moveSpeed = 2;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1;
    public int edgeIterations = 4;
    public float edgeDistanceThreshold = 0.5f;

    public Transform[] waypoints;
    int m_currentWaypointIndex = 0;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_targetPosition;

    bool m_IsPatrol = true;
    bool m_CaughtPlayer = false;
    bool m_PlayerInRanger = false;
    bool m_PlayerNear = false;
    float m_WaitTime;
    float m_TimeToRotate;
    Vector3 m_PlayerPosition = Vector3.zero;

    void Start()
    {
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        m_currentWaypointIndex = 0;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_currentWaypointIndex].position);
    }

    void Update()
    {
        EnvironmentView();
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
                if (m_WaitTime <= 0 && !m_CaughtPlayer && distance >= 6f)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;
                    Move(speedWalk);
                    m_TimeToRotate = timeToRotate;
                    m_WaitTime = startWaitTime;
                    navMeshAgent.SetDestination(waypoints[m_currentWaypointIndex].position);
                }
                else
                {
                    if (distance >= 2.5f)
                    {
                        Stop();
                        m_WaitTime -= Time.deltaTime;
                    }
                }
            }
        }
    }

    private void Patrolling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_currentWaypointIndex].position);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_currentWaypointIndex = (m_currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_currentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) < 0.5f)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_currentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                m_PlayerInRanger = true;
                m_IsPatrol = false;
            }
            else
            {
                m_PlayerInRanger = false;
            }

            if (Vector3.Distance(transform.position, target.position) > viewRadius)
            {
                m_PlayerInRanger = false;
            }
        }

        if (!m_PlayerInRanger)
        {
            m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }
}
