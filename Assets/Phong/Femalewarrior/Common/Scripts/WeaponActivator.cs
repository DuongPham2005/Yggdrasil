using UnityEngine;

public class WeaponActivator : MonoBehaviour
{
    public Collider weaponCollider;

    void Start()
    {
        weaponCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        weaponCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        weaponCollider.enabled = false;
    }
}
