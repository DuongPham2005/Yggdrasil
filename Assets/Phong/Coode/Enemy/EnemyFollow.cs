using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Animator anim;
    public float range;

    public float timeRandomMin;
    public float timeRandomMax;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isWalking", false);
        float distance = range;
        float dist = Vector3.Distance(enemy.transform.position, player.position);
        if (dist < distance)
        {
            Invoke("setEnemy", Random.Range(timeRandomMin, timeRandomMax));
        }
    }
    void setEnemy()
    {
        anim.SetBool("isWalking", true);
        enemy.SetDestination(player.position); 
 
    }
}
