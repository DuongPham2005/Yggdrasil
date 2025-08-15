using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptSSS.InventorySystem;

public class GoldDisplay : MonoBehaviour
{
    [Header("Gold UI")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image goldIcon;
    [SerializeField] private bool showIcon = true;
    
    [Header("Display Settings")]
    [SerializeField] private string prefix = "";
    [SerializeField] private string suffix = "";
    [SerializeField] private bool useThousandSeparator = true;
    
    private CurrencyWallet playerWallet;
    
    void Start()
    {
        // Tìm CurrencyWallet của player
        playerWallet = FindObjectOfType<ScriptSSS.InventorySystem.CurrencyWallet>();
        
        if (playerWallet != null)
        {
            // Subscribe to gold events
            playerWallet.OnGoldChanged.AddListener(OnGoldChanged);
            
            // Hiển thị gold ban đầu
            UpdateGoldDisplay();
            
            Debug.Log($"GoldDisplay: Đã kết nối với CurrencyWallet, gold hiện tại: {playerWallet.Gold}");
        }
        else
        {
            Debug.LogError("GoldDisplay: Không tìm thấy CurrencyWallet! Hãy đảm bảo Player có component này.");
        }
        
        // Ẩn icon nếu không muốn hiển thị
        if (goldIcon != null)
        {
            goldIcon.gameObject.SetActive(showIcon);
        }
    }
    
    void Update()
    {
        // Có thể thêm logic cập nhật real-time nếu cần
        // Hiện tại chỉ cập nhật khi có event
    }
    
    /// <summary>
    /// Cập nhật hiển thị gold
    /// </summary>
    private void UpdateGoldDisplay()
    {
        if (goldText == null || playerWallet == null) return;
        
        int currentGold = playerWallet.Gold;
        string formattedGold;
        
        if (useThousandSeparator)
        {
            // Format với dấu phẩy ngăn cách hàng nghìn
            formattedGold = currentGold.ToString("N0");
        }
        else
        {
            formattedGold = currentGold.ToString();
        }
        
        // Tạo text cuối cùng với prefix và suffix
        goldText.text = $"{prefix}{formattedGold}{suffix}";
    }
    
    /// <summary>
    /// Event handler khi gold thay đổi
    /// </summary>
    /// <param name="newGoldAmount">Số gold mới</param>
    public void OnGoldChanged(int newGoldAmount)
    {
        UpdateGoldDisplay();
        
        // Có thể thêm animation hoặc hiệu ứng ở đây
        if (goldText != null)
        {
            // Ví dụ: Flash effect
            StartCoroutine(FlashGoldText());
        }
    }
    
    /// <summary>
    /// Hiệu ứng flash cho gold text
    /// </summary>
    private System.Collections.IEnumerator FlashGoldText()
    {
        if (goldText == null) yield break;
        
        Color originalColor = goldText.color;
        Color flashColor = Color.yellow;
        
        // Flash 3 lần
        for (int i = 0; i < 3; i++)
        {
            goldText.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            goldText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    /// <summary>
    /// Cập nhật gold display từ bên ngoài
    /// </summary>
    public void RefreshDisplay()
    {
        UpdateGoldDisplay();
    }
    
    /// <summary>
    /// Thiết lập prefix cho gold text
    /// </summary>
    /// <param name="newPrefix">Prefix mới</param>
    public void SetPrefix(string newPrefix)
    {
        prefix = newPrefix;
        UpdateGoldDisplay();
    }
    
    /// <summary>
    /// Thiết lập suffix cho gold text
    /// </summary>
    /// <param name="newSuffix">Suffix mới</param>
    public void SetSuffix(string newSuffix)
    {
        suffix = newSuffix;
        UpdateGoldDisplay();
    }
    
    /// <summary>
    /// Bật/tắt hiển thị icon
    /// </summary>
    /// <param name="show">Có hiển thị icon không</param>
    public void SetIconVisibility(bool show)
    {
        showIcon = show;
        if (goldIcon != null)
        {
            goldIcon.gameObject.SetActive(show);
        }
    }
    
    /// <summary>
    /// Bật/tắt dấu phẩy ngăn cách hàng nghìn
    /// </summary>
    /// <param name="useSeparator">Có sử dụng dấu phẩy không</param>
    public void SetThousandSeparator(bool useSeparator)
    {
        useThousandSeparator = useSeparator;
        UpdateGoldDisplay();
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (playerWallet != null)
        {
            playerWallet.OnGoldChanged.RemoveListener(OnGoldChanged);
        }
    }
    
    // Context menu để test từ Inspector
    [ContextMenu("Refresh Gold Display")]
    public void TestRefresh()
    {
        RefreshDisplay();
    }
    
    [ContextMenu("Test Flash Effect")]
    public void TestFlash()
    {
        if (goldText != null)
        {
            StartCoroutine(FlashGoldText());
        }
    }
}
