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
            animator.SetInteger("AttackTree", attackType);

            if (attackType == 1)
                animator.SetBool("isAttack", true);
            else
                animator.SetBool("isAttack2", true);
        }
        else if (distance < detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            animator.SetBool("isWalking", speed > 0.1f && speed < 3f);
            animator.SetBool("isRunning", speed >= 3f);

            animator.SetBool("isAttack", false);
            animator.SetBool("isAttack2", false);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttack", false);
            animator.SetBool("isAttack2", false);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        animator.SetTrigger("GetHit");

        if (hp <= 0)
        {
            animator.SetTrigger("Die");
        }
    }
}

