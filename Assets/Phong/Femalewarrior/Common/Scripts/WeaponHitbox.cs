using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
        }
    }
}
