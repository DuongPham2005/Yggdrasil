using UnityEngine;
using ScriptSSS.InventorySystem;

[RequireComponent(typeof(Collider))]
public class ShopNPC : Interactable
{
	[Header("Config")]
	public ItemSO rubyItem;   // bán ruby => đưa vàng cho người chơi
	public int rubySellPrice = 10; // mỗi ruby bán được bao nhiêu vàng

	public ItemSO meatItem;   // mua thịt để hồi máu
	public int meatBuyPrice = 5;   // giá một miếng thịt
	public float meatHealAmount = 25f; // hồi bao nhiêu máu khi dùng ngay

	[Header("Refs (auto)")]
	private Inventory inventory;
	private CurrencyWallet wallet;
	private HealthSystem healthSystem;

	public override void Start()
	{
		base.Start();
	}

	protected override void Interaction()
	{
		base.Interaction();
		if (inventory == null || wallet == null || healthSystem == null)
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			if (player != null)
			{
				inventory = player.GetComponentInChildren<Inventory>();
				wallet = player.GetComponentInChildren<CurrencyWallet>();
				healthSystem = player.GetComponentInChildren<HealthSystem>();
			}
		}

		if (inventory == null || wallet == null)
		{
			Debug.LogWarning("Player is missing Inventory or CurrencyWallet.");
			return;
		}

		// 1) Bán toàn bộ ruby trong túi => nhận vàng
		if (rubyItem != null)
		{
			int rubyCount = inventory.Count(rubyItem);
			if (rubyCount > 0)
			{
				int goldGain = rubyCount * Mathf.Max(0, rubySellPrice);
				inventory.Remove(rubyItem, rubyCount);
				wallet.AddGold(goldGain);
				Debug.Log($"Sold {rubyCount} ruby for {goldGain} gold. Current gold: {wallet.Gold}");
			}
			else
			{
				Debug.Log("No ruby to sell.");
			}
		}

		// 2) Mua 1 miếng thịt nếu đủ tiền, và dùng ngay để hồi máu
		if (meatItem != null)
		{
			if (wallet.TrySpendGold(Mathf.Max(0, meatBuyPrice)))
			{
				inventory.Add(meatItem, 1);
				Debug.Log($"Bought 1 meat for {meatBuyPrice} gold. Gold left: {wallet.Gold}");
				if (healthSystem != null && meatHealAmount > 0f)
				{
					healthSystem.Heal(meatHealAmount);
				}
			}
			else
			{
				Debug.Log("Not enough gold to buy meat.");
			}
		}
	}
}


