using UnityEngine;
using UnityEngine.AI;

public class GoblinAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public int attackType = 1; // Random hoặc cố định
    public int hp = 100;

    private NavMeshAgent agent;
    private Animator animator;

    public float attackDamage = 10f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (hp <= 0)
        {
            animator.SetTrigger("Die");
            agent.isStopped = true;
            this.enabled = false;
            return;
        }

        if (distance < attackRange)
        {
            agent.isStopped = true;
            transform.LookAt(player);

            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

            animator.SetFloat("Blend", attackType == 1 ? 0f : 1f);
            animator.SetTrigger("Attack");

            //// Gây sát thương mỗi lần tấn công (dễ gây bug đánh quá nhiều)
            //PlayerHealth ph = player.GetComponent<PlayerHealth>();
            //if (ph != null)
            //{
            //    ph.TakeDamage(10);  // ⚠️ Điều này sẽ gọi liên tục trong mỗi frame!
            //}
        }

        else if (distance < detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            animator.SetBool("isWalking", speed > 0.1f && speed < 3f);
            animator.SetBool("isRunning", speed >= 3f);

            // Dừng Attack khi đuổi theo
            animator.ResetTrigger("Attack");
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.ResetTrigger("Attack");
        }
    }
    public void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(attackDamage);
            }
        }
    }

}

