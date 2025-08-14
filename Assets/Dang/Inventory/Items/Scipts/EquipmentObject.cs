using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Unity.FantasyKingdom
{
    [CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
    public class EquipmentObject : ItemObject
    {
        public float atkBonus;
        public float defenceBonus;
        public void Awake()
        {
            type = ItemType.Equipment;
        }
    }
}
