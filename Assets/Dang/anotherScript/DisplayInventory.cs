using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
namespace Unity.FantasyKingdom
{
    public class DisplayInventory : MonoBehaviour
    {
        public InventoryObject inventory;
        public int X_START;
        public int Y_START;
        public int X_SPACE_BETWEEN_ITEM;
        public int Y_SPACE_BETWEEN_ITEM;
        public int NUMBER_OF_COLUM;
        Dictionary<InventorySlot, GameObject> itemDisplayd = new Dictionary<InventorySlot, GameObject>();

        void Start()
        {
            CreateDisplay();
        }

        private void Update()
        {
            UpdateDisplay();
        }
        public void UpdateDisplay()
        {
            for (int i = 0; i < inventory.Container.Count; i++)
            {
                if (itemDisplayd.ContainsKey(inventory.Container[i]))
                {
                    itemDisplayd[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                }
                else
                {
                    var obj = Instantiate(inventory.Container[i].Item.prefab, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                    itemDisplayd.Add(inventory.Container[i], obj);
                }
            }
        }

        public void CreateDisplay()
        {
            for (int i = 0; i < inventory.Container.Count; i++)
            {
                var obj = Instantiate(inventory.Container[i].Item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemDisplayd.Add(inventory.Container[i], obj);
            }
        }
         public Vector3 GetPosition(int i)
        {
            return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUM)), Y_START + (-Y_SPACE_BETWEEN_ITEM *(i/NUMBER_OF_COLUM)), 0f);
        }
    }
}
