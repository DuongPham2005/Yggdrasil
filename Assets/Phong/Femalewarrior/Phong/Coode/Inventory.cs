using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("Đã thêm vật phẩm: " + item.itemName);
    }
}
