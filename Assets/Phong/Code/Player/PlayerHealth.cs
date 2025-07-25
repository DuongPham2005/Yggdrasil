//using Retro.ThirdPersonCharacter;
//using UnityEngine;

//public class PlayerHealth : MonoBehaviour
//{
//    public int maxHealth = 100;
//    private int currentHealth;

//    private Animator animator;

//    void Start()
//    {
//        currentHealth = maxHealth;
//        animator = GetComponent<Animator>();
//    }

//    public void TakeDamage(int amount)
//    {
//        if (currentHealth <= 0) return; // Đã chết thì không xử lý nữa

//        currentHealth -= amount;
//        Debug.Log("Player bị đánh! Máu còn: " + currentHealth);

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//        else
//        {
//            animator.SetBool("IsHit", true); // Bật animation trúng đòn
//        }
//    }

//    void Die()
//    {
//        Debug.Log("Player chết");

//        animator.SetBool("Die", true); // Chạy animation chết

//        // Vô hiệu hóa điều khiển
//        var move = GetComponent<Movement>();
//        if (move != null) move.enabled = false;

//        var combat = GetComponent<Combat>();
//        if (combat != null) combat.enabled = false;
//    }

//    // Gọi từ animation event cuối clip trúng đòn
//    public void EndHitReact()
//    {
//        animator.SetBool("IsHit", false);
//    }
//}
