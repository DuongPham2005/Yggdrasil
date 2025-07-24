using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class NPCController : MonoBehaviour
    {
        public float wanderRadius = 10f;
        public float wanderTimer = 3f;
        public float idleTime = 2f;

        private NavMeshAgent agent;
        private Animator animator;
        private float timer;
        private float idleTimer;
        private bool isIdle = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = wanderTimer;
        }

        void Update()
        {
            if (isIdle)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime)
                {
                    isIdle = false;
                    timer = wanderTimer;
                }
                SetAnimation(false);
                return;
            }

            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isIdle = true;
                idleTimer = 0;
            }

            SetAnimation(agent.velocity.magnitude > 0.1f);
        }

        Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
            return navHit.position;
        }

        void SetAnimation(bool isMoving)
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", isMoving);
            }
        }
    }
}
