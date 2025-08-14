using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.InventorySystem
{
	[CreateAssetMenu(menuName = "Inventory/ItemDatabase", fileName = "ItemDatabase")]
	public class ItemDatabase : ScriptableObject
	{
		[SerializeField] private List<ItemSO> items = new List<ItemSO>();

		private Dictionary<string, ItemSO> idToItem = new Dictionary<string, ItemSO>();

		private void OnEnable()
		{
			RebuildIndex();
		}

		public void RebuildIndex()
		{
			idToItem.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				var it = items[i];
				if (it == null || string.IsNullOrEmpty(it.id)) continue;
				if (!idToItem.ContainsKey(it.id)) idToItem.Add(it.id, it);
			}
		}

		public ItemSO GetById(string id)
		{
			if (string.IsNullOrEmpty(id)) return null;
			if (idToItem.Count == 0) RebuildIndex();
			return idToItem.TryGetValue(id, out var item) ? item : null;
		}

		public IReadOnlyList<ItemSO> Items => items;
	}
}


