using UnityEngine;
using UnityEngine.Events;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 15f; // Stamina per second
    public float staminaRegenDelay = 1f; // Delay before regen starts after using stamina
    
    [Header("Stamina Costs")]
    public float sprintCost = 20f; // Stamina per second while sprinting
    public float attackCost = 25f; // Stamina per attack
    public float jumpCost = 15f; // Stamina per jump
    
    [Header("UI References")]
    public UnityEngine.UI.Slider staminaSlider;
    public UnityEngine.UI.Image staminaFillImage;
    public Color fullStaminaColor = Color.yellow;
    public Color lowStaminaColor = Color.red;
    public Color staminaBackgroundColor = Color.gray;
    
    [Header("Events")]
    public UnityEvent<float> OnStaminaChanged;
    public UnityEvent OnStaminaDepleted;
    
    private float currentStamina;
    private float lastStaminaUseTime;
    private bool isStaminaDepleted = false;
    
    public float CurrentStamina => currentStamina;
    public float StaminaPercentage => currentStamina / maxStamina;
    public bool IsStaminaDepleted => isStaminaDepleted;
    public bool HasStaminaForSprint => currentStamina >= sprintCost * Time.deltaTime;
    public bool HasStaminaForAttack => currentStamina >= attackCost;
    public bool HasStaminaForJump => currentStamina >= jumpCost;

    void Start()
    {
        currentStamina = maxStamina;
        lastStaminaUseTime = -staminaRegenDelay; // Allow immediate regen at start
        UpdateStaminaUI();
    }
    
    void Update()
    {
        if (currentStamina < maxStamina)
        {
            // Check if enough time has passed since last stamina use
            if (Time.time - lastStaminaUseTime >= staminaRegenDelay)
            {
                RegenerateStamina();
            }
        }
    }

    public bool UseStaminaForSprint()
    {
        if (isStaminaDepleted || currentStamina < sprintCost * Time.deltaTime)
        {
            return false;
        }
        
        float cost = sprintCost * Time.deltaTime;
        currentStamina -= cost;
        lastStaminaUseTime = Time.time;
        
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        if (currentStamina <= 0)
        {
            DepleteStamina();
        }
        
        return true;
    }
    
    public bool UseStaminaForAttack()
    {
        if (isStaminaDepleted || currentStamina < attackCost)
        {
            return false;
        }
        
        currentStamina -= attackCost;
        lastStaminaUseTime = Time.time;
        
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        Debug.Log($"Used {attackCost} stamina for attack. Current stamina: {currentStamina}");
        
        if (currentStamina <= 0)
        {
            DepleteStamina();
        }
        
        return true;
    }
    
    public bool UseStaminaForJump()
    {
        if (isStaminaDepleted || currentStamina < jumpCost)
        {
            return false;
        }
        
        currentStamina -= jumpCost;
        lastStaminaUseTime = Time.time;
        
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        Debug.Log($"Used {jumpCost} stamina for jump. Current stamina: {currentStamina}");
        
        if (currentStamina <= 0)
        {
            DepleteStamina();
        }
        
        return true;
    }
    
    public void UseStamina(float amount)
    {
        if (isStaminaDepleted || currentStamina < amount)
        {
            return;
        }
        
        currentStamina -= amount;
        lastStaminaUseTime = Time.time;
        
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        if (currentStamina <= 0)
        {
            DepleteStamina();
        }
    }
    
    public void RestoreStamina(float amount)
    {
        currentStamina = Mathf.Min(maxStamina, currentStamina + amount);
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        if (currentStamina > 0 && isStaminaDepleted)
        {
            isStaminaDepleted = false;
        }
    }
    
    private void RegenerateStamina()
    {
        float regenAmount = staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(maxStamina, currentStamina + regenAmount);
        
        if (currentStamina == maxStamina)
        {
            return; // Don't update UI if already at max
        }
        
        OnStaminaChanged?.Invoke(currentStamina);
        UpdateStaminaUI();
        
        if (currentStamina > 0 && isStaminaDepleted)
        {
            isStaminaDepleted = false;
        }
    }
    
    private void DepleteStamina()
    {
        if (isStaminaDepleted) return;
        
        isStaminaDepleted = true;
        currentStamina = 0;
        UpdateStaminaUI();
        
        OnStaminaDepleted?.Invoke();
        Debug.Log("Stamina depleted!");
    }
    
    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = StaminaPercentage;
        }
        
        if (staminaFillImage != null)
        {
            staminaFillImage.color = Color.Lerp(lowStaminaColor, fullStaminaColor, StaminaPercentage);
        }
        
        // Set background color
        if (staminaSlider != null && staminaSlider.GetComponentInChildren<UnityEngine.UI.Image>() != null)
        {
            var backgroundImage = staminaSlider.GetComponentInChildren<UnityEngine.UI.Image>();
            if (backgroundImage != staminaFillImage)
            {
                backgroundImage.color = staminaBackgroundColor;
            }
        }
    }
} 