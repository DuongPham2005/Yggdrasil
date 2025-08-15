using UnityEngine;
using UnityEngine.Events;

namespace ScriptSSS.InventorySystem
{
	public class CurrencyWallet : MonoBehaviour
	{
		[SerializeField] private int gold;
		
		[Header("Events")]
		public UnityEvent<int> OnGoldChanged;

		public int Gold => gold;

		public void AddGold(int amount)
		{
			if (amount <= 0) return;
			gold += amount;
			OnGoldChanged?.Invoke(gold);
		}

		public bool TrySpendGold(int amount)
		{
			if (amount <= 0) return true;
			if (gold < amount) return false;
			gold -= amount;
			OnGoldChanged?.Invoke(gold);
			return true;
		}
		
		// Method để set gold trực tiếp (cho testing hoặc save/load)
		public void SetGold(int newAmount)
		{
			if (newAmount < 0) newAmount = 0;
			gold = newAmount;
			OnGoldChanged?.Invoke(gold);
		}
	}
}


