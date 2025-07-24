using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Unity.FantasyKingdom
{
    public class PlayerInventory : MonoBehaviour
    {
        public InventoryObject inventory;

        public void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item)
            {
                inventory.AddItem(item.item, 1);
                Destroy(other.gameObject);
            }
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.L)) 
            {
                inventory.Save();
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                inventory.Load();
            }
        }
        private void OnApplicationQuit()
        {
            inventory.Container.Clear();
        }
    }
    
}
