using UnityEngine;
using System.Collections;

public class BossSke : MonoBehaviour
{
    [Header("VFX Prefabs")]
    public GameObject auraVFX;
    public GameObject meteorVFX;
    public GameObject fireballVFX;

    [Header("Aura Settings")]
    public Transform auraSpawnPoint;
    public float auraDuration = 2f;
    public float auraScale = 1f;

    [Header("Meteor Settings")]
    public Transform meteorSpawnPoint;
    public float meteorDamage = 20f;
    public float meteorHitRadius = 2f;
    private float lastMeteorHealthTrigger = 1f;

    [Header("Fireball Settings")]
    public Transform fireballSpawnPoint;
    public float fireballSpawnInterval = 10f;
    public float fireballDamage = 15f;
    public float fireballSpeed = 8f;

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

        // Tính % máu boss dựa vào Enemy.health
        float healthPercent = enemyScript.health / enemyScript.maxHealth;

        // Meteor + Aura mỗi khi mất 10% máu
        if (healthPercent <= lastMeteorHealthTrigger - 0.1f)
        {
            lastMeteorHealthTrigger -= 0.1f;
            StartCoroutine(Skill_AuraMeteor());
        }

        // Fireball bắn liên tục
        if (!isSpawningFireball)
        {
            StartCoroutine(Skill_FireballLoop());
        }
    }

    IEnumerator Skill_AuraMeteor()
    {
        // Spawn Aura cảnh báo
        Vector3 auraPos = auraSpawnPoint ? auraSpawnPoint.position : transform.position;
        Quaternion auraRot = auraSpawnPoint ? auraSpawnPoint.rotation : transform.rotation;

        GameObject aura = Instantiate(auraVFX, auraPos, auraRot);
        aura.transform.localScale *= auraScale;
        Debug.Log("Boss tung Aura cảnh báo");

        // Lock vị trí player
        Vector3 targetPos = player.position;

        yield return new WaitForSeconds(auraDuration);

        // Spawn Meteor
        Vector3 meteorPos = meteorSpawnPoint ? meteorSpawnPoint.position : targetPos;
        GameObject meteor = Instantiate(meteorVFX, meteorPos, Quaternion.identity);
        Debug.Log("Boss triệu hồi Meteor tại: " + meteorPos);

        // Damage nếu player trong bán kính
        if (Vector3.Distance(player.position, meteorPos) <= meteorHitRadius)
        {
            ApplyDamage(player.gameObject, 20f);
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

    // === Trừ máu player qua HealthSystem ===
    private void ApplyDamage(GameObject target, float damage)
    {
        HealthSystem playerHP = target.GetComponent<HealthSystem>();
        if (playerHP != null)
        {
            playerHP.TakeDamage(damage);
            Debug.Log("Player nhận " + damage + " damage từ Boss");
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
