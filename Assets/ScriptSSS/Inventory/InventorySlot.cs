using System;
using UnityEngine;

namespace ScriptSSS.InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public ItemSO item;
        public int quantity;

        public bool IsEmpty => item == null || quantity <= 0;
        public bool IsFull => !IsEmpty && item.stackable && quantity >= item.maxStack;

        public bool CanStack(ItemSO other) =>
            !IsEmpty && item == other && item.stackable && quantity < item.maxStack;

        public void Clear()
        {
            item = null;
            quantity = 0;
        }
    }
}
