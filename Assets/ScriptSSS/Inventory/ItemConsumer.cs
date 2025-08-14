using UnityEngine;

namespace ScriptSSS.InventorySystem
{
	public class ItemConsumer : MonoBehaviour
	{
		[Header("Config")]
		public KeyCode consumeKey = KeyCode.H;
		public ItemSO meatItem;
		public float healAmount = 25f;

		[Header("Auto References")]
		public Inventory inventory; // auto-find if null
		public HealthSystem healthSystem; // auto-find if null

		AudioSource audioSource;
		public AudioClip useSound;

		void Awake()
		{
			if (inventory == null) inventory = GetComponentInChildren<Inventory>();
			if (healthSystem == null) healthSystem = GetComponentInChildren<HealthSystem>();
			audioSource = GetComponent<AudioSource>();
		}

		void Update()
		{
			if (Input.GetKeyDown(consumeKey))
			{
				TryConsumeMeat();
			}
		}

		public bool TryConsumeMeat()
		{
			if (inventory == null || meatItem == null || healthSystem == null)
			{
				Debug.LogWarning("ItemConsumer missing references (inventory/meatItem/healthSystem).");
				return false;
			}

			int count = inventory.Count(meatItem);
			if (count <= 0)
			{
				// No meat in inventory
				return false;
			}

			bool removed = inventory.Remove(meatItem, 1);
			if (!removed) return false;

			healthSystem.Heal(Mathf.Max(0f, healAmount));
			if (audioSource != null && useSound != null)
			{
				audioSource.PlayOneShot(useSound);
			}
			return true;
		}
	}
}


