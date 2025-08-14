using System;
using System.Collections.Generic;
using UnityEngine;
using ScriptSSS.InventorySystem;

namespace ScriptSSS.SaveLoad
{
	[Serializable]
	public class InventorySlotData
	{
		public string itemId;
		public int quantity;
	}

	[Serializable]
	public class PlayerData
	{
		public Vector3 position;
		public Quaternion rotation;
		public float health;
		public float maxHealth;
		public float stamina;
		public float maxStamina;
	}

	[Serializable]
	public class SettingsData
	{
		public float masterVolume = 1f;
		public float musicVolume = 1f;
		public float sfxVolume = 1f;
		public int qualityLevel = -1; // -1 keep
		public int targetFrameRate = 60;
	}

	[Serializable]
	public class GameSaveData
	{
		public string saveId = Guid.NewGuid().ToString();
		public string sceneName;
		public long savedUnixTime;

		public PlayerData player = new PlayerData();
		public int inventoryCapacity;
		public List<InventorySlotData> inventory = new List<InventorySlotData>();
		public SettingsData settings = new SettingsData();
	}
}


