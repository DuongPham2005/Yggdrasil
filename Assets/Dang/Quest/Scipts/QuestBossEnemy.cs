using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class QuestBossEnemy : MonoBehaviour
    {
        public GameObject dropItemPrefab; 

        public void Die()
        {
            QuestBossManager manager = FindObjectOfType<QuestBossManager>();
            manager.BossDefeated();

            if (dropItemPrefab != null)
            {
                Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
