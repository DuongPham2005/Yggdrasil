using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject vfxPrefab;     
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float lifeTime = 2f;     

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
        Destroy(vfx, lifeTime); 
        vfx.AddComponent<ProjectileMover>().Init(projectileSpeed);
        
        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        
    }
}

// Script phụ để điều khiển bay
public class ProjectileMover : MonoBehaviour
{
    float speed;
    public void Init(float spd) => speed = spd;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
