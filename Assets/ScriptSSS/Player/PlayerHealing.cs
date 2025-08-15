using UnityEngine;
using System.Collections;

public class PlayerHealing : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private KeyCode healKey = KeyCode.H;
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private float healCooldown = 3f;
    [SerializeField] private bool canHeal = true;
    
    [Header("VFX")]
    [SerializeField] private GameObject healVFX;
    [SerializeField] private Transform healSpawnPoint;
    [SerializeField] private float vfxDuration = 2f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip healSound;
    
    [Header("UI Feedback")]
    [SerializeField] private bool showHealText = true;
    [SerializeField] private string healMessage = "Healed!";
    
    private HealthSystem playerHealth;
    private float lastHealTime;
    private bool isHealing = false;
    
    void Start()
    {
        // Tìm HealthSystem của player
        playerHealth = GetComponent<HealthSystem>();
        
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<HealthSystem>();
        }
        
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealing: Không tìm thấy HealthSystem! Hãy đảm bảo Player có component này.");
        }
        
        // Tìm AudioSource nếu không được gán
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // Nếu không có healSpawnPoint, sử dụng vị trí player
        if (healSpawnPoint == null)
        {
            healSpawnPoint = transform;
        }
    }
    
    void Update()
    {
        if (!canHeal || playerHealth == null) return;
        
        // Kiểm tra cooldown
        if (Time.time - lastHealTime < healCooldown) return;
        
        // Kiểm tra input phím H
        if (Input.GetKeyDown(healKey))
        {
            TryHeal();
        }
    }
    
    /// <summary>
    /// Thử hồi máu cho player
    /// </summary>
    public void TryHeal()
    {
        if (isHealing) return;
        
        // Kiểm tra xem player có cần hồi máu không
        if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
        {
            Debug.Log("Player đã đầy máu, không cần hồi!");
            return;
        }
        
        // Bắt đầu quá trình hồi máu
        StartCoroutine(HealPlayer());
    }
    
    /// <summary>
    /// Coroutine để hồi máu với VFX
    /// </summary>
    private IEnumerator HealPlayer()
    {
        isHealing = true;
        
        // Spawn VFX healing
        if (healVFX != null)
        {
            Vector3 spawnPos = healSpawnPoint.position;
            GameObject vfx = Instantiate(healVFX, spawnPos, Quaternion.identity);
            
            // Đảm bảo VFX hướng lên trên
            vfx.transform.rotation = Quaternion.identity;
            
            // Tự động destroy VFX sau một thời gian
            Destroy(vfx, vfxDuration);
            
            Debug.Log("Đã spawn VFX healing!");
        }
        
        // Play sound healing
        if (audioSource != null && healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }
        
        // Hiển thị text feedback
        if (showHealText)
        {
            StartCoroutine(ShowHealText());
        }
        
        // Chờ một chút để VFX bắt đầu
        yield return new WaitForSeconds(0.1f);
        
        // Thực hiện hồi máu
        playerHealth.Heal(healAmount);
        
        // Cập nhật thời gian hồi máu cuối
        lastHealTime = Time.time;
        
        // Chờ VFX hoàn thành
        yield return new WaitForSeconds(vfxDuration - 0.1f);
        
        isHealing = false;
        
        Debug.Log($"Đã hồi {healAmount} máu cho player!");
    }
    
    /// <summary>
    /// Hiển thị text feedback khi hồi máu
    /// </summary>
    private IEnumerator ShowHealText()
    {
        // Có thể thêm UI text hoặc world space text ở đây
        // Ví dụ: Floating text, UI notification, etc.
        
        // Tạm thời chỉ log ra console
        Debug.Log($"<color=green>{healMessage}</color>");
        
        yield return null;
    }
    
    /// <summary>
    /// Hồi máu từ bên ngoài (không cần input)
    /// </summary>
    /// <param name="amount">Lượng máu hồi</param>
    public void ForceHeal(float amount)
    {
        if (playerHealth != null && !isHealing)
        {
            StartCoroutine(ForceHealCoroutine(amount));
        }
    }
    
    /// <summary>
    /// Coroutine để force heal
    /// </summary>
    private IEnumerator ForceHealCoroutine(float amount)
    {
        isHealing = true;
        
        // Spawn VFX
        if (healVFX != null)
        {
            Vector3 spawnPos = healSpawnPoint.position;
            GameObject vfx = Instantiate(healVFX, spawnPos, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }
        
        // Play sound
        if (audioSource != null && healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }
        
        yield return new WaitForSeconds(0.1f);
        
        // Heal
        playerHealth.Heal(amount);
        
        yield return new WaitForSeconds(vfxDuration - 0.1f);
        
        isHealing = false;
    }
    
    /// <summary>
    /// Bật/tắt khả năng hồi máu
    /// </summary>
    /// <param name="enabled">Có thể hồi máu không</param>
    public void SetHealingEnabled(bool enabled)
    {
        canHeal = enabled;
    }
    
    /// <summary>
    /// Thiết lập cooldown mới
    /// </summary>
    /// <param name="newCooldown">Cooldown mới (giây)</param>
    public void SetHealCooldown(float newCooldown)
    {
        healCooldown = Mathf.Max(0f, newCooldown);
    }
    
    /// <summary>
    /// Thiết lập lượng máu hồi mới
    /// </summary>
    /// <param name="newAmount">Lượng máu hồi mới</param>
    public void SetHealAmount(float newAmount)
    {
        healAmount = Mathf.Max(0f, newAmount);
    }
    
    /// <summary>
    /// Kiểm tra xem có thể hồi máu không
    /// </summary>
    /// <returns>True nếu có thể hồi máu</returns>
    public bool CanHeal()
    {
        if (!canHeal) return false;
        if (Time.time - lastHealTime < healCooldown) return false;
        if (playerHealth == null) return false;
        if (playerHealth.CurrentHealth >= playerHealth.MaxHealth) return false;
        
        return true;
    }
    
    /// <summary>
    /// Lấy thời gian còn lại của cooldown
    /// </summary>
    /// <returns>Thời gian còn lại (giây)</returns>
    public float GetCooldownRemaining()
    {
        float timeSinceLastHeal = Time.time - lastHealTime;
        float remaining = healCooldown - timeSinceLastHeal;
        return Mathf.Max(0f, remaining);
    }
    
    // Context menu để test từ Inspector
    [ContextMenu("Test Heal")]
    public void TestHeal()
    {
        TryHeal();
    }
    
    [ContextMenu("Force Heal 50")]
    public void TestForceHeal()
    {
        ForceHeal(50f);
    }
    
    [ContextMenu("Reset Cooldown")]
    public void ResetCooldown()
    {
        lastHealTime = -healCooldown;
    }
}
