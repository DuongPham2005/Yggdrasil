using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    //this mob
    [SerializeField] protected Transform target;
    [SerializeField] protected NavMeshAgent navMeshAgent;
        //enemy and player
    public NavMeshAgent enemy;
    public Transform player;
    [SerializeField] protected Animator anim;
    public float range;
    public bool isWalking = false;
    public bool isRunning = false;

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
    protected virtual void LoadComponents()
    {
 
        this.LoadAgent();
        this.LoadAnimator();

    }
    protected virtual void LoadAgent()
    {
        if (this.navMeshAgent != null) return;
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(transform.name + ": LoadAgent", gameObject);
    }
    protected virtual void LoadAnimator()
    {
        if (this.anim != null) return;
        this.anim = GetComponent<Animator>();
        Debug.Log(transform.name + ": LoadAnimator", gameObject);
    }

    public void Moving()
    {
        if(this.target == null)
        {
            this.navMeshAgent.isStopped = true;
            this.isWalking = false;
            return;
        }
        this.isWalking = true;
        this.navMeshAgent.isStopped = false;
        this.navMeshAgent.SetDestination(this.target.position);
    }
    protected virtual void setEnemy()
    {
        anim.SetBool("isRunning", true);
        enemy.SetDestination(player.position); 
 
    }
}
