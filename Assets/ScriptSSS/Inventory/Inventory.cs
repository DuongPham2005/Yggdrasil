using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int capacity = 20;
        [SerializeField] private List<InventorySlot> slots;

        public event Action OnChanged;

        public int Capacity => capacity;
        public IReadOnlyList<InventorySlot> Slots => slots;

        private void Awake()
        {
            if (capacity < 1) capacity = 1;
            if (slots == null || slots.Count != capacity)
            {
                slots = new List<InventorySlot>(capacity);
                for (int i = 0; i < capacity; i++) slots.Add(new InventorySlot());
            }
        }

        public bool Add(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            // Stack vào các slot có sẵn
            for (int i = 0; i < slots.Count && amount > 0; i++)
            {
                var s = slots[i];
                if (s.CanStack(item))
                {
                    int canAdd = Mathf.Min(item.maxStack - s.quantity, amount);
                    s.quantity += canAdd;
                    amount -= canAdd;
                }
            }

            // Điền vào ô trống
            for (int i = 0; i < slots.Count && amount > 0; i++)
            {
                var s = slots[i];
                if (s.IsEmpty)
                {
                    s.item = item;
                    int add = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;
                    s.quantity = add;
                    amount -= add;
                }
            }

            bool success = amount == 0;
            if (success) OnChanged?.Invoke();
            return success;
        }

        public bool Remove(ItemSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            for (int i = 0; i < slots.Count && amount > 0; i++)
            {
                var s = slots[i];
                if (!s.IsEmpty && s.item == item)
                {
                    int take = Mathf.Min(s.quantity, amount);
                    s.quantity -= take;
                    amount -= take;
                    if (s.quantity <= 0) s.Clear();
            }
            }

            bool success = amount == 0;
            if (success) OnChanged?.Invoke();
            return success;
        }

        public int Count(ItemSO item)
        {
            if (item == null) return 0;
            int total = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (!s.IsEmpty && s.item == item) total += s.quantity;
            }
            return total;
        }

		// Save/Load helpers
		public void ClearAll()
		{
			if (slots == null) return;
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].Clear();
			}
			OnChanged?.Invoke();
		}

		public void SetSlot(int index, ItemSO item, int quantity)
		{
			if (slots == null || index < 0 || index >= slots.Count) return;
			slots[index].item = item;
			slots[index].quantity = Mathf.Max(0, quantity);
			if (slots[index].quantity == 0) slots[index].Clear();
			OnChanged?.Invoke();
		}

		public void EnsureCapacity(int desiredCapacity)
		{
			if (desiredCapacity < 1) desiredCapacity = 1;
			if (slots == null) slots = new List<InventorySlot>();
			capacity = desiredCapacity;
			if (slots.Count < capacity)
			{
				for (int i = slots.Count; i < capacity; i++) slots.Add(new InventorySlot());
			}
			else if (slots.Count > capacity)
			{
				slots.RemoveRange(capacity, slots.Count - capacity);
			}
			OnChanged?.Invoke();
		}
    }
}
