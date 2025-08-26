using System.Collections;
using UnityEngine;

public class BossLightningSkill : MonoBehaviour
{
    [Header("Lightning Settings")]
    public GameObject lightningPrefab;    // Prefab sét đánh
    public float strikeInterval = 1f;     // delay giữa các tia sét
    public float strikeDamage = 10f;      // damage mỗi lần
    public float strikeRadius = 2f;       // bán kính gây damage
    public LayerMask playerLayer;         // layer Player

    [Header("Strike Position Settings")]
    public float radius = 5f;             // bán kính vòng tròn (ngũ giác)

    [Header("Cooldown Settings")]
    public float skillCooldown = 5f;      // thời gian hồi chiêu (5 giây)
    private float nextCastTime = 0f;      // mốc thời gian lần tiếp theo

    void Update()
    {
        if (Time.time >= nextCastTime)
        {
            CastLightning();
            nextCastTime = Time.time + skillCooldown;
        }
    }

    public void CastLightning()
    {
        Debug.Log(">>> Boss bắt đầu tung chiêu sét!");
        StartCoroutine(LightningRoutine());
    }

    IEnumerator LightningRoutine()
    {
        // Tính toán 5 điểm ngũ giác quanh boss
        Vector3[] strikePositions = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            float angle = i * Mathf.PI * 2f / 5f; // chia vòng tròn thành 5 phần
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            strikePositions[i] = transform.position + dir * radius;
        }

        // Đánh sét lần lượt từng điểm
        for (int i = 0; i < strikePositions.Length; i++)
        {
            Vector3 pos = strikePositions[i];
            Debug.Log($"Boss gọi sét lần {i + 1} tại {pos}");

            // Spawn VFX sét từ trên cao
            if (lightningPrefab != null)
            {
                GameObject lightning = Instantiate(lightningPrefab, pos + Vector3.up * 0f, Quaternion.identity);
                Destroy(lightning, 2f);
            }
            else
            {
                Debug.LogWarning("⚠ Chưa gắn prefab Lightning!");
            }

            // Check player trong vùng damage
            Collider[] hits = Physics.OverlapSphere(pos, strikeRadius, playerLayer);
            foreach (Collider hit in hits)
            {
                HealthSystem hp = hit.GetComponent<HealthSystem>();
                if (hp != null)
                {
                    hp.TakeDamage(strikeDamage);
                    Debug.Log($"⚡ Player {hit.name} trúng sét lần {i + 1}, -{strikeDamage} HP");
                }
                else
                {
                    Debug.LogWarning($"⚠ Collider {hit.name} trong vùng nhưng không có HealthSystem!");
                }
            }

            yield return new WaitForSeconds(strikeInterval);
        }

        Debug.Log(">>> Boss đã tung xong chiêu sét!");
    }

    // Gizmos để debug vị trí 5 điểm
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 5; i++)
        {
            float angle = i * Mathf.PI * 2f / 5f;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 pos = transform.position + dir * radius;
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}
