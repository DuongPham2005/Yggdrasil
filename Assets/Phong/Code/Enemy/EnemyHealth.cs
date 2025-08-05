using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;

    private Animator animator;
    private NavMeshAgent agent;
    private GoblinAI goblinAI;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        goblinAI = GetComponent<GoblinAI>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health > 0)
        {
            animator.SetTrigger("GetHit"); // ✅ Gọi animation GetHit
        }

        if (health <= 0)
        {
            // Gọi chết
            animator.SetTrigger("Die");

            if (goblinAI != null)
                goblinAI.enabled = false;

            if (agent != null)
                agent.isStopped = true;

            Destroy(gameObject, 2f); // Xoá quái sau 2 giây
        }
    }
}
