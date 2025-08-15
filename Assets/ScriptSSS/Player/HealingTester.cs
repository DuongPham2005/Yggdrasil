using UnityEngine;

public class HealingTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private KeyCode damageKey = KeyCode.J;
    [SerializeField] private KeyCode resetHealthKey = KeyCode.K;
    
    [Header("Healing Info")]
    [SerializeField] private bool showHealingInfo = true;
    
    private HealthSystem playerHealth;
    private PlayerHealing playerHealing;
    
    void Start()
    {
        // Tìm components cần thiết
        playerHealth = FindObjectOfType<HealthSystem>();
        playerHealing = FindObjectOfType<PlayerHealing>();
        
        if (playerHealth == null)
        {
            Debug.LogError("HealingTester: Không tìm thấy HealthSystem!");
        }
        
        if (playerHealing == null)
        {
            Debug.LogError("HealingTester: Không tìm thấy PlayerHealing!");
        }
        
        // Hiển thị thông tin healing
        if (showHealingInfo)
        {
            ShowHealingInfo();
        }
    }
    
    void Update()
    {
        if (playerHealth == null) return;
        
        // Nhấn J để gây damage
        if (Input.GetKeyDown(damageKey))
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log($"Đã gây {damageAmount} damage cho player. HP hiện tại: {playerHealth.CurrentHealth}");
        }
        
        // Nhấn K để reset health về max
        if (Input.GetKeyDown(resetHealthKey))
        {
            playerHealth.SetHealth(playerHealth.MaxHealth);
            Debug.Log($"Đã reset HP về {playerHealth.MaxHealth}");
        }
        
        // Hiển thị thông tin healing mỗi giây
        if (showHealingInfo && playerHealing != null)
        {
            ShowHealingStatus();
        }
    }
    
    /// <summary>
    /// Hiển thị thông tin về hệ thống healing
    /// </summary>
    private void ShowHealingInfo()
    {
        if (playerHealing == null) return;
        
        Debug.Log("=== HỆ THỐNG HEALING ===");
        Debug.Log("Nhấn H để hồi máu");
        Debug.Log("Nhấn J để gây damage (test)");
        Debug.Log("Nhấn K để reset HP (test)");
        Debug.Log("========================");
    }
    
    /// <summary>
    /// Hiển thị trạng thái healing
    /// </summary>
    private void ShowHealingStatus()
    {
        if (playerHealth == null || playerHealing == null) return;
        
        string status = $"HP: {playerHealth.CurrentHealth:F0}/{playerHealth.MaxHealth:F0}";
        
        if (playerHealing.CanHeal())
        {
            status += " | ✅ Có thể hồi máu";
        }
        else
        {
            float cooldownRemaining = playerHealing.GetCooldownRemaining();
            if (cooldownRemaining > 0)
            {
                status += $" | ⏰ Cooldown: {cooldownRemaining:F1}s";
            }
            else if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            {
                status += " | 💚 Đã đầy máu";
            }
        }
        
        // Chỉ log khi có thay đổi để tránh spam
        if (Time.frameCount % 60 == 0) // Mỗi 60 frames (1 giây)
        {
            Debug.Log(status);
        }
    }
    
    // Context menu để test từ Inspector
    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
    
    [ContextMenu("Test Reset Health")]
    public void TestResetHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.SetHealth(playerHealth.MaxHealth);
        }
    }
    
    [ContextMenu("Test Heal")]
    public void TestHeal()
    {
        if (playerHealing != null)
        {
            playerHealing.TryHeal();
        }
    }
    
    [ContextMenu("Show Healing Info")]
    public void ShowInfo()
    {
        ShowHealingInfo();
    }
}
