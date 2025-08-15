using UnityEngine;
using ScriptSSS.InventorySystem;

public class GoldTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private int addGoldAmount = 100;
    [SerializeField] private int spendGoldAmount = 50;
    [SerializeField] private KeyCode addGoldKey = KeyCode.G;
    [SerializeField] private KeyCode spendGoldKey = KeyCode.H;
    [SerializeField] private KeyCode resetGoldKey = KeyCode.R;
    
    private CurrencyWallet playerWallet;
    
    void Start()
    {
        // Tìm wallet của player
        playerWallet = FindObjectOfType<CurrencyWallet>();
        
        if (playerWallet == null)
        {
            Debug.LogError("Không tìm thấy CurrencyWallet! Hãy đảm bảo Player có component này.");
        }
        else
        {
            Debug.Log($"Tìm thấy CurrencyWallet với {playerWallet.Gold} gold");
        }
    }
    
    void Update()
    {
        if (playerWallet == null) return;
        
        // Nhấn G để thêm gold
        if (Input.GetKeyDown(addGoldKey))
        {
            playerWallet.AddGold(addGoldAmount);
            Debug.Log($"Đã thêm {addGoldAmount} gold. Tổng: {playerWallet.Gold}");
        }
        
        // Nhấn H để tiêu gold
        if (Input.GetKeyDown(spendGoldKey))
        {
            bool success = playerWallet.TrySpendGold(spendGoldAmount);
            if (success)
            {
                Debug.Log($"Đã tiêu {spendGoldAmount} gold. Còn lại: {playerWallet.Gold}");
            }
            else
            {
                Debug.LogWarning($"Không đủ gold để tiêu {spendGoldAmount}! Hiện có: {playerWallet.Gold}");
            }
        }
        
        // Nhấn R để reset gold về 0
        if (Input.GetKeyDown(resetGoldKey))
        {
            playerWallet.SetGold(0);
            Debug.Log("Đã reset gold về 0");
        }
    }
    
    // Method để test từ Inspector
    [ContextMenu("Add Gold")]
    public void TestAddGold()
    {
        if (playerWallet != null)
        {
            playerWallet.AddGold(addGoldAmount);
        }
    }
    
    [ContextMenu("Spend Gold")]
    public void TestSpendGold()
    {
        if (playerWallet != null)
        {
            playerWallet.TrySpendGold(spendGoldAmount);
        }
    }
    
    [ContextMenu("Reset Gold")]
    public void TestResetGold()
    {
        if (playerWallet != null)
        {
            playerWallet.SetGold(0);
        }
    }
}
