using UnityEngine;

public class HealthTestController : MonoBehaviour
{
    [Header("Test Controls")]
    public KeyCode damageKey = KeyCode.H;
    public KeyCode healKey = KeyCode.J;
    public KeyCode staminaDrainKey = KeyCode.K;
    public KeyCode staminaRestoreKey = KeyCode.L;
    
    [Header("Test Values")]
    public float damageAmount = 10f;
    public float healAmount = 20f;
    public float staminaDrainAmount = 30f;
    public float staminaRestoreAmount = 50f;
    
    private HealthSystem playerHealth;
    private PlayerStamina playerStamina;
    
    void Start()
    {
        playerHealth = GetComponent<HealthSystem>();
        playerStamina = GetComponent<PlayerStamina>();
    }
    
    void Update()
    {
        // Test Health System
        if (Input.GetKeyDown(damageKey))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"Test: Dealt {damageAmount} damage to player");
            }
        }
        
        if (Input.GetKeyDown(healKey))
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log($"Test: Healed {healAmount} HP to player");
            }
        }
        
        // Test Stamina System
        if (Input.GetKeyDown(staminaDrainKey))
        {
            if (playerStamina != null)
            {
                playerStamina.UseStamina(staminaDrainAmount);
                Debug.Log($"Test: Drained {staminaDrainAmount} stamina from player");
            }
        }
        
        if (Input.GetKeyDown(staminaRestoreKey))
        {
            if (playerStamina != null)
            {
                playerStamina.RestoreStamina(staminaRestoreAmount);
                Debug.Log($"Test: Restored {staminaRestoreAmount} stamina to player");
            }
        }
    }
    
    void OnGUI()
    {
        // Display test instructions
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("Health & Stamina Test Controls:");
        GUILayout.Label($"Press {damageKey} to take {damageAmount} damage");
        GUILayout.Label($"Press {healKey} to heal {healAmount} HP");
        GUILayout.Label($"Press {staminaDrainKey} to drain {staminaDrainAmount} stamina");
        GUILayout.Label($"Press {staminaRestoreKey} to restore {staminaRestoreAmount} stamina");
        
        if (playerHealth != null)
        {
            GUILayout.Label($"Health: {playerHealth.CurrentHealth:F1}/{playerHealth.MaxHealth}");
        }
        
        if (playerStamina != null)
        {
            GUILayout.Label($"Stamina: {playerStamina.CurrentStamina:F1}/{playerStamina.maxStamina}");
        }
        
        GUILayout.EndArea();
    }
} 