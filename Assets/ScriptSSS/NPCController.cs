using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class NPCController : MonoBehaviour
    {
        public float wanderRadius = 10f;
        public float wanderTimer = 3f;
        public float idleTime = 2f;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public int attackDamage = 10;

        private NavMeshAgent agent;
        private Animator animator;
        private float timer;
        private float idleTimer;
        private bool isIdle = false;
        private float lastAttackTime;
        private Transform player;
        private PlayerHealth playerHealth;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = wanderTimer;
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }
        }

        void Update()
        {
            // Liên tục cập nhật vị trí player
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                    playerHealth = playerObj.GetComponent<PlayerHealth>();
                }
            }

            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance > attackRange)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    SetAnimation(agent.velocity.magnitude > 0.1f);
                }
                else
                {
                    agent.isStopped = true;
                    SetAnimation(false);
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        if (animator) {
                            animator.SetTrigger("Attack");
                            Debug.Log("Boss Attack Triggered!");
                        }
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(attackDamage);
                        }
                        lastAttackTime = Time.time;
                    }
                }
                return;
            }

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

        public void DealDamage()
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);
        }
    }
}
