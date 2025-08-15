using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float lifeTime = 2f;
    public float damage = 1f; // hệ số damage chỉnh được

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootVFX();
        }
    }

    void ShootVFX()
    {
        GameObject vfx = Instantiate(vfxPrefab, firePoint.position, firePoint.rotation);
        vfx.transform.forward = firePoint.forward;

        // Gắn script điều khiển bay và gây damage
        ProjectileMover mover = vfx.AddComponent<ProjectileMover>();
        mover.Init(projectileSpeed, lifeTime, damage);

        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
    }
}

// Script phụ để điều khiển bay + xử lý va chạm
public class ProjectileMover : MonoBehaviour
{
    float speed;
    float lifeTime;
    float damage;

    public void Init(float spd, float lt, float dmg)
    {
        speed = spd;
        lifeTime = lt;
        damage = dmg;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Nếu va chạm với enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"Hit enemy: {enemy.name} - Damage: {damage}");
            Destroy(gameObject); // Hủy VFX sau khi trúng
        }
    }
}
