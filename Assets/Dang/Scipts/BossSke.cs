using UnityEngine;
using System.Collections;

public class BossSke : MonoBehaviour
{
    [Header("VFX Prefabs")]
    public GameObject fireballVFX;

    [Header("Fireball Settings")]
    public Transform fireballSpawnPoint;
    public float fireballSpawnInterval = 10f;
    public float fireballDamage = 15f;
    public float fireballSpeed = 8f;

    [Header("Aggro Settings")]
    public float aggroRange = 4f; // Vùng để bắn fireball

    private Enemy enemyScript; // Boss kế thừa từ Enemy
    private Transform player;
    private bool isSpawningFireball = false;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (enemyScript == null || player == null) return;

        // Fireball chỉ bắn khi player trong vùng Aggro Range
        if (!isSpawningFireball && IsPlayerInAggroRange())
        {
            StartCoroutine(Skill_FireballLoop());
        }
    }



    IEnumerator Skill_FireballLoop()
    {
        isSpawningFireball = true;
        yield return new WaitForSeconds(fireballSpawnInterval);

        if (player != null)
        {
            Vector3 spawnPos = fireballSpawnPoint ? fireballSpawnPoint.position : transform.position;
            Vector3 dir = (player.position - spawnPos).normalized;

            GameObject fireball = Instantiate(fireballVFX, spawnPos, Quaternion.LookRotation(dir));
            Rigidbody rb = fireball.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity = dir * fireballSpeed;

            // Nếu Fireball có script damage
            FireballDamage dmgScript = fireball.GetComponentInChildren<FireballDamage>();
            if (dmgScript != null)
            {
                dmgScript.SetDamage(fireballDamage);
                Debug.Log("Đã set damage cho Fireball: " + fireballDamage);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy FireballDamage trên prefab!");
            }
        }

        isSpawningFireball = false;
    }

    // === Kiểm tra player có trong vùng Aggro Range không ===
    private bool IsPlayerInAggroRange()
    {
        if (player == null) return false;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool inRange = distanceToPlayer <= aggroRange;
        
        // Debug log để kiểm tra
        if (inRange)
        {
            Debug.Log($"Player trong vùng Aggro Range: {distanceToPlayer:F1}/{aggroRange}");
        }
        
        return inRange;
    }

    // === Hiển thị vùng Aggro Range trong Scene view (chỉ để debug) ===
    private void OnDrawGizmosSelected()
    {
        // Vẽ vùng Aggro Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        
        // Vẽ đường nối đến player nếu có
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
            
            // Hiển thị khoảng cách
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= aggroRange)
            {
                Gizmos.color = Color.green; // Xanh khi trong range
            }
            else
            {
                Gizmos.color = Color.red; // Đỏ khi ngoài range
            }
            Gizmos.DrawWireSphere(player.position, 0.5f);
        }
    }



    public class FireballDamage : MonoBehaviour
    {
        private float damage;

        public void SetDamage(float dmg)
        {
            damage = dmg;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Fireball va chạm với: " + other.name); 
            if (other.CompareTag("Player"))
            {
                HealthSystem playerHP = other.GetComponent<HealthSystem>();
                if (playerHP != null)
                {
                    playerHP.TakeDamage(damage);
                    Debug.Log("Player bị Fireball trúng! Mất " + damage + " máu");
                }
                Destroy(gameObject);
            }
        }
    }

}
