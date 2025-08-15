using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Health UI")]
    public Slider healthSlider;
    public Image healthFillImage;
    public TextMeshProUGUI healthText;
    
    [Header("Stamina UI")]
    public Slider staminaSlider;
    public Image staminaFillImage;
    public TextMeshProUGUI staminaText;
    

    
    [Header("UI Animation")]
    public float updateSpeed = 5f;
    public bool showText = true;
    
    private HealthSystem playerHealth;
    private PlayerStamina playerStamina;
    private float targetHealthValue;
    private float targetStaminaValue;
    
    void Start()
    {
        // Tìm HealthSystem và PlayerStamina components
        playerHealth = FindObjectOfType<HealthSystem>();
        playerStamina = FindObjectOfType<PlayerStamina>();
        
        if (playerHealth != null)
        {
            // Subscribe to health events
            playerHealth.OnHealthChanged.AddListener(OnHealthChanged);
            playerHealth.OnPlayerDied.AddListener(OnPlayerDied);
            
            // Setup health UI
            if (healthSlider != null)
            {
                healthSlider.maxValue = 1f;
                healthSlider.value = playerHealth.HealthPercentage;
                targetHealthValue = playerHealth.HealthPercentage;
            }
            
            if (healthFillImage != null)
            {
                healthFillImage.color = Color.Lerp(playerHealth.lowHealthColor, playerHealth.fullHealthColor, playerHealth.HealthPercentage);
            }
            
            UpdateHealthText();
        }
        
        if (playerStamina != null)
        {
            // Subscribe to stamina events
            playerStamina.OnStaminaChanged.AddListener(OnStaminaChanged);
            playerStamina.OnStaminaDepleted.AddListener(OnStaminaDepleted);
            
            // Setup stamina UI
            if (staminaSlider != null)
            {
                staminaSlider.maxValue = 1f;
                staminaSlider.value = playerStamina.StaminaPercentage;
                targetStaminaValue = playerStamina.StaminaPercentage;
            }
            
            if (staminaFillImage != null)
            {
                staminaFillImage.color = Color.Lerp(playerStamina.lowStaminaColor, playerStamina.fullStaminaColor, playerStamina.StaminaPercentage);
            }
            
            UpdateStaminaText();
        }
    }
    
    void Update()
    {
        // Smooth UI updates
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealthValue, updateSpeed * Time.deltaTime);
        }
        
        if (staminaSlider != null)
        {
            staminaSlider.value = Mathf.Lerp(staminaSlider.value, targetStaminaValue, updateSpeed * Time.deltaTime);
        }
    }
    
    private void OnHealthChanged(float newHealth)
    {
        if (playerHealth != null)
        {
            targetHealthValue = playerHealth.HealthPercentage;
            
            if (healthFillImage != null)
            {
                healthFillImage.color = Color.Lerp(playerHealth.lowHealthColor, playerHealth.fullHealthColor, playerHealth.HealthPercentage);
            }
            
            UpdateHealthText();
        }
    }
    
    private void OnStaminaChanged(float newStamina)
    {
        if (playerStamina != null)
        {
            targetStaminaValue = playerStamina.StaminaPercentage;
            
            if (staminaFillImage != null)
            {
                staminaFillImage.color = Color.Lerp(playerStamina.lowStaminaColor, playerStamina.fullStaminaColor, playerStamina.StaminaPercentage);
            }
            
            UpdateStaminaText();
        }
    }
    
    private void OnPlayerDied()
    {
        Debug.Log("Player died - UI updated");
        // Có thể thêm hiệu ứng UI khi player chết
        // Ví dụ: Fade out, show game over screen, etc.
    }
    
    private void OnStaminaDepleted()
    {
        Debug.Log("Stamina depleted - UI updated");
        // Có thể thêm hiệu ứng UI khi stamina hết
        // Ví dụ: Flash red, shake, etc.
    }
    
    private void UpdateHealthText()
    {
        if (healthText != null && showText && playerHealth != null)
        {
            healthText.text = $"{Mathf.RoundToInt(playerHealth.CurrentHealth)}/{playerHealth.MaxHealth}";
        }
    }
    
    private void UpdateStaminaText()
    {
        if (staminaText != null && showText && playerStamina != null)
        {
            staminaText.text = $"{Mathf.RoundToInt(playerStamina.CurrentStamina)}/{playerStamina.maxStamina}";
        }
    }
    

    
    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(OnHealthChanged);
            playerHealth.OnPlayerDied.RemoveListener(OnPlayerDied);
        }
        
        if (playerStamina != null)
        {
            playerStamina.OnStaminaChanged.RemoveListener(OnStaminaChanged);
            playerStamina.OnStaminaDepleted.RemoveListener(OnStaminaDepleted);
        }
    }
} 