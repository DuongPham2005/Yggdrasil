using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public NPCInteraction npc;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npc.CollectItem();  // Gọi hàm trong NPC
            Destroy(gameObject);
        }
    }
}
