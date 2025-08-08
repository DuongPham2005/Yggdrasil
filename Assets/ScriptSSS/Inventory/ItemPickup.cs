using UnityEngine;

namespace ScriptSSS.InventorySystem
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        public ItemSO item;
        public int amount = 1;
        public string playerTag = "Player";
        public KeyCode pickupKey = KeyCode.E;

        private Inventory currentInventory;
        private bool playerInside;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                currentInventory = other.GetComponentInParent<Inventory>();
                playerInside = currentInventory != null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInside = false;
                currentInventory = null;
            }
        }

        private void Update()
        {
            if (!playerInside || currentInventory == null) return;
            if (Input.GetKeyDown(pickupKey))
            {
                if (currentInventory.Add(item, Mathf.Max(1, amount)))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
