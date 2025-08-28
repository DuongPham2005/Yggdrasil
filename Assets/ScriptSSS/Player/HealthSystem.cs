using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health = 100f;
    [SerializeField] float healthRegenRate = 5f; // HP per second
    [SerializeField] float healthRegenDelay = 3f; // Delay before regen starts after taking damage
    [SerializeField] bool autoRegenEnabled = false; // allow toggling regen (disabled by request)
    
    [Header("UI References")]
    public UnityEngine.UI.Slider healthSlider;
    public UnityEngine.UI.Image healthFillImage;
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public Color healthBackgroundColor = Color.gray;
    
    [Header("VFX")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;
    
    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnPlayerDied;
    
    [Header("Death UI")]
    [SerializeField] private GameObject deathPanel; // Panel hiển thị khi chết
    
    private Animator animator;
    private float lastDamageTime;
    private bool isDead = false;
    
    public float CurrentHealth => health;
    public float MaxHealth => maxHealth;
    public float HealthPercentage => health / maxHealth;
    public bool IsDead => isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        lastDamageTime = -healthRegenDelay; // Allow immediate regen at start
        UpdateHealthUI();
        
        // Ẩn death panel khi bắt đầu game
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        if (autoRegenEnabled && !isDead && health < maxHealth)
        {
            // Check if enough time has passed since last damage
            if (Time.time - lastDamageTime >= healthRegenDelay)
            {
                RegenerateHealth();
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        
        health = Mathf.Max(0, health - damageAmount);
        lastDamageTime = Time.time;
        
        animator.SetTrigger("damage");
        OnHealthChanged?.Invoke(health);
        UpdateHealthUI();
        
        Debug.Log($"Player took {damageAmount} damage. Current HP: {health}");
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (isDead) return;
        
        health = Mathf.Min(maxHealth, health + amount);
        OnHealthChanged?.Invoke(health);
        UpdateHealthUI();
        
        Debug.Log($"Player healed {amount} HP. Current HP: {health}");
    }

    // Save/Load helpers
    public void SetHealth(float newHealth, float? newMaxHealth = null)
    {
        if (newMaxHealth.HasValue)
        {
            maxHealth = Mathf.Max(1f, newMaxHealth.Value);
        }
        health = Mathf.Clamp(newHealth, 0f, maxHealth);
        isDead = health <= 0f;
        OnHealthChanged?.Invoke(health);
        UpdateHealthUI();
    }
    
    private void RegenerateHealth()
    {
        float regenAmount = healthRegenRate * Time.deltaTime;
        health = Mathf.Min(maxHealth, health + regenAmount);
        
        if (health == maxHealth)
        {
            return; // Don't update UI if already at max
        }
        
        OnHealthChanged?.Invoke(health);
        UpdateHealthUI();
    }

    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        health = 0;
        UpdateHealthUI();
        
        OnPlayerDied?.Invoke();
        Debug.Log("Player died!");
        
        // Spawn ragdoll if available
        if (ragdoll != null)
        {
            Instantiate(ragdoll, transform.position, transform.rotation);
        }
        
        // Show death panel if available
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }
    

    
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = HealthPercentage;
        }
        
        if (healthFillImage != null)
        {
            healthFillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, HealthPercentage);
        }
        
        // Set background color
        if (healthSlider != null && healthSlider.GetComponentInChildren<UnityEngine.UI.Image>() != null)
        {
            var backgroundImage = healthSlider.GetComponentInChildren<UnityEngine.UI.Image>();
            if (backgroundImage != healthFillImage)
            {
                backgroundImage.color = healthBackgroundColor;
            }
        }
    }
    
    public void HitVFX(Vector3 hitPosition)
    {
        if (hitVFX != null)
        {
            GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
            Destroy(hit, 1f);
        }
    }
    
    // Method để ẩn death panel (gọi khi respawn)
    public void HideDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }
    
    // Method để hiển thị death panel
    public void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }
}