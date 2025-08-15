using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Blood Rain Settings")]
    [SerializeField] GameObject bloodRainPrefab;
    [SerializeField] Transform bloodRainSpawnPoint;
    [SerializeField] float bloodRainDamage = 2f;
    [SerializeField] float bloodRainTick = 1f;

    [Header("Tornado Settings")]
    [SerializeField] GameObject tornadoPrefab;
    [SerializeField] Transform tornadoSpawnPoint;
    [SerializeField] float tornadoSpeed = 5f;
    [SerializeField] float tornadoCooldown = 5f;
    [SerializeField] float tornadoDamage = 10f;
    [SerializeField] float tornadoKnockback = 5f;

    private GameObject player;
    private Enemy enemyScript; // Lấy máu từ script Enemy
    private bool bloodRainActivated = false;
    private float tornadoTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyScript = GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError("Boss không tìm thấy script Enemy trên cùng GameObject!");
            enabled = false;
            return;
        }

        tornadoTimer = tornadoCooldown;
    }

    void Update()
    {
        if (player == null || enemyScript == null) return;

        float currentHealth = enemyScript.health;
        float maxHealth = enemyScript.maxHealth;

        // Kích hoạt mưa máu khi máu <= 50%
        if (!bloodRainActivated && currentHealth <= maxHealth / 2f)
        {
            CastBloodRain();
            bloodRainActivated = true;
        }

        // Cooldown tung lốc xoáy
        tornadoTimer -= Time.deltaTime;
        if (tornadoTimer <= 0f)
        {
            CastTornado();
            tornadoTimer = tornadoCooldown;
        }
    }

    void CastBloodRain()
    {
        Debug.Log("Boss tung chiêu Mưa Máu");
        GameObject rain = Instantiate(bloodRainPrefab, bloodRainSpawnPoint.position, Quaternion.identity);
        rain.AddComponent<BloodRainDamage>().Init(bloodRainDamage, bloodRainTick, "Player");
    }

    void CastTornado()
    {
        Debug.Log("Boss tung chiêu Lốc Xoáy");
        GameObject tornado = Instantiate(tornadoPrefab, tornadoSpawnPoint.position, tornadoSpawnPoint.rotation);

        Rigidbody rb = tornado.GetComponent<Rigidbody>();
        if (rb == null) rb = tornado.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = (player.transform.position - tornadoSpawnPoint.position).normalized * tornadoSpeed;

        tornado.AddComponent<TornadoDamage>().Init(tornadoDamage, tornadoKnockback, "Player");
        Destroy(tornado, 10f);
    }
}

public class BloodRainDamage : MonoBehaviour
{
    float damage;
    float tickRate;
    string targetTag;

    public void Init(float dmg, float tick, string tag)
    {
        damage = dmg;
        tickRate = tick;
        targetTag = tag;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            StartCoroutine(DamageOverTime(other));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DamageOverTime(Collider target)
    {
        HealthSystem hp = target.GetComponent<HealthSystem>();
        while (true)
        {
            Debug.Log("Player đang đứng trong Mưa Máu");
            if (hp != null) hp.TakeDamage(damage);
            yield return new WaitForSeconds(tickRate);
        }
    }
}

public class TornadoDamage : MonoBehaviour
{
    float damage;
    float knockbackForce;
    string targetTag;

    public void Init(float dmg, float knockback, string tag)
    {
        damage = dmg;
        knockbackForce = knockback;
        targetTag = tag;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Debug.Log("Player bị dính Lốc Xoáy");

            HealthSystem hp = collision.gameObject.GetComponent<HealthSystem>();
            if (hp != null) hp.TakeDamage(damage);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 dir = (collision.transform.position - transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
