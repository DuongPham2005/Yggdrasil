using UnityEngine;
using UnityEngine.VFX;

namespace Unity.FantasyKingdom
{
    public class FireballDamage : MonoBehaviour
    {
        private float damage = 15f;
        private bool GotHit = false;
        public VisualEffect vfxPrefab;
        public GameObject objectToDisable;
        public Rigidbody rb;
        public void SetDamage(float dmg)
        {
            damage = dmg;
        }

        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
          
            if (other.CompareTag("Player"))
            {
                HealthSystem playerHP = other.GetComponent<HealthSystem>();
                if (playerHP != null)
                {
                    playerHP.TakeDamage(damage);
                    Debug.Log("Player bị Fireball trúng! Mất " + damage + " máu");
                }
                Destroy(gameObject, 5f);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (GotHit == false)
            {
               
                if (vfxPrefab != null)
                {
                   
                    VisualEffect vfxInstance = Instantiate(vfxPrefab, collision.contacts[0].point, Quaternion.identity);

                    
                    vfxInstance.SendEvent("OnPlay");

                  
                    Destroy(vfxInstance.gameObject, 1f);

                }

                
                if (objectToDisable != null)
                {

                  
                    Destroy(this.gameObject);
                    GotHit = true;


                }

            }
        }
    }
    

}
