using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class NPCController : MonoBehaviour
    {
        // Vị trí trung tâm vùng di chuyển
        private Vector3 centerPosition;
        // Bán kính vùng di chuyển
        public float moveRadius = 5f;
        // Tốc độ di chuyển
        public float moveSpeed = 2f;
        // Thời gian dừng lại khi đến điểm
        public float waitTime = 2f;

        private Vector3 targetPosition;
        private bool isWaiting = false;
        private float waitTimer = 0f;
        private Animator animator;
        private Quaternion targetRotation;
        public float rotationSpeed = 5f;
        public float detectPlayerRadius = 7f;
        public float attackDistance = 1.5f;
        private Transform player;
        private bool chasingPlayer = false;
        private NavMeshAgent agent;
        public float attackCooldown = 2f;
        private float attackTimer = 0f;
        private bool isAttacking = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            centerPosition = transform.position;
            PickNewTargetPosition();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = moveSpeed;
                agent.stoppingDistance = attackDistance;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Tìm player nếu chưa có
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
            }

            // Kiểm tra khoảng cách tới player
            if (player != null)
            {
                float playerDist = Vector3.Distance(transform.position, player.position);
                if (playerDist <= detectPlayerRadius)
                {
                    chasingPlayer = true;
                }
                else if (playerDist > detectPlayerRadius + 1f)
                {
                    chasingPlayer = false;
                }
            }

            if (chasingPlayer && player != null)
            {
                if (agent != null)
                {
                    agent.stoppingDistance = attackDistance;
                    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                    if (distanceToPlayer > attackDistance)
                    {
                        // Player ngoài phạm vi tấn công, tiếp tục di chuyển
                        agent.isStopped = false;
                        agent.SetDestination(player.position);
                        if (animator != null) animator.SetBool("IsWalking", true);
                        isAttacking = false;
                        attackTimer = 0f;
                    }
                    else
                    {
                        // Player trong phạm vi tấn công
                        agent.isStopped = true;
                        if (animator != null) animator.SetBool("IsWalking", false);
                        // Tấn công liên tục nếu hết thời gian hồi chiêu
                        if (!isAttacking && attackTimer <= 0f)
                        {
                            if (animator != null) animator.SetTrigger("Attack");
                            isAttacking = true;
                            attackTimer = attackCooldown;
                            // TODO: Gây sát thương cho player ở đây nếu muốn
                        }
                    }
                }
                // Đếm ngược thời gian hồi chiêu
                if (isAttacking)
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        isAttacking = false;
                        attackTimer = 0f;
                    }
                }
                return;
            }

            if (isWaiting)
            {
                if (animator != null) animator.SetBool("IsWalking", false);
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    PickNewTargetPosition();
                }
                if (agent != null) agent.isStopped = true;
                return;
            }

            // Tuần tra
            if (agent != null)
            {
                agent.isStopped = false;
                agent.stoppingDistance = 0.1f;
                agent.SetDestination(targetPosition);
                if (agent.remainingDistance < 0.2f)
                {
                    isWaiting = true;
                    waitTimer = waitTime;
                    if (animator != null) animator.SetBool("IsWalking", false);
                }
                else
                {
                    if (animator != null) animator.SetBool("IsWalking", true);
                }
            }
        }

        void PickNewTargetPosition()
        {
            // Chọn điểm ngẫu nhiên trong hình tròn quanh centerPosition
            Vector2 randomCircle = Random.insideUnitCircle * moveRadius;
            targetPosition = centerPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
        }
    }
}
