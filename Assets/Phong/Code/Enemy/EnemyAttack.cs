using UnityEngine;

// Gắn script này vào Enemy hoặc vũ khí của enemy
public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Gửi sát thương đến player
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Kích hoạt animation bị đánh bên Player
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Hit"); // cần tạo trigger "Hit" trong Animator của Player
            }
        }
    }
}

