using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptSSS.InventorySystem;

namespace ScriptSSS.SaveLoad
{
	public class SaveManager : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform playerTransform;
		[SerializeField] private HealthSystem playerHealth;
		[SerializeField] private PlayerStamina playerStamina;
		[SerializeField] private Inventory playerInventory;
		[SerializeField] private ItemDatabase itemDatabase;
		[SerializeField] private SettingsManager settingsManager;

		[Header("Options")]
		[SerializeField] private string defaultSlot = "slot1";

		public void SaveGame(string slotName = null)
		{
			var data = new GameSaveData();
			data.sceneName = SceneManager.GetActiveScene().name;
			data.savedUnixTime = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			// Player transform
			if (playerTransform != null)
			{
				data.player.position = playerTransform.position;
				data.player.rotation = playerTransform.rotation;
			}

			// Health & Stamina
			if (playerHealth != null)
			{
				data.player.health = playerHealth.CurrentHealth;
				data.player.maxHealth = playerHealth.MaxHealth;
			}
			if (playerStamina != null)
			{
				data.player.stamina = playerStamina.CurrentStamina;
				data.player.maxStamina = playerStamina.maxStamina;
			}

			// Inventory
			if (playerInventory != null)
			{
				data.inventoryCapacity = playerInventory.Capacity;
				var slots = playerInventory.Slots;
				for (int i = 0; i < slots.Count; i++)
				{
					var s = slots[i];
					data.inventory.Add(new InventorySlotData
					{
						itemId = s.IsEmpty ? null : s.item.id,
						quantity = s.IsEmpty ? 0 : s.quantity
					});
				}
			}

			// Settings
			if (settingsManager != null)
			{
				data.settings = settingsManager.Capture();
			}

			SaveSystem.SaveJson(data, string.IsNullOrEmpty(slotName) ? defaultSlot : slotName);
		}

		public bool LoadGame(string slotName = null)
		{
			if (!SaveSystem.TryLoadJson(string.IsNullOrEmpty(slotName) ? defaultSlot : slotName, out GameSaveData data))
			{
				Debug.LogWarning("No save found");
				return false;
			}

			// Note: If scene differs, you might want to SceneManager.LoadScene(data.sceneName) then restore in OnSceneLoaded.

			// Player transform
			if (playerTransform != null)
			{
				playerTransform.SetPositionAndRotation(data.player.position, data.player.rotation);
			}

			// Health & Stamina
			if (playerHealth != null)
			{
				playerHealth.SetHealth(data.player.health, data.player.maxHealth);
			}
			if (playerStamina != null)
			{
				playerStamina.SetStamina(data.player.stamina, data.player.maxStamina);
			}

			// Inventory
			if (playerInventory != null)
			{
				playerInventory.EnsureCapacity(data.inventoryCapacity > 0 ? data.inventoryCapacity : playerInventory.Capacity);
				playerInventory.ClearAll();
				if (itemDatabase == null)
				{
					Debug.LogWarning("ItemDatabase is not assigned; inventory will be empty after load.");
				}
				for (int i = 0; i < data.inventory.Count && i < playerInventory.Capacity; i++)
				{
					var slot = data.inventory[i];
					ItemSO item = itemDatabase != null ? itemDatabase.GetById(slot.itemId) : null;
					playerInventory.SetSlot(i, item, slot.quantity);
				}
			}

			// Settings
			if (settingsManager != null && data.settings != null)
			{
				settingsManager.Apply(data.settings);
			}

			return true;
		}
	}
}


