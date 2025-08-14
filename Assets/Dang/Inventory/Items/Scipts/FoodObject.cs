using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity.FantasyKingdom
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
    public class FoodObject : ItemObject
    {
        public int restoreHealthvalue; 
        public void Awake()
        {
            type = ItemType.Food;
        }
    }
}
