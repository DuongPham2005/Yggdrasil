using UnityEngine;

public class CombatStaminaController : MonoBehaviour
{
    [Header("Combat Stamina Settings")]
    public float lightAttackCost = 15f;
    public float heavyAttackCost = 35f;
    public float blockCost = 10f; // Stamina per second while blocking
    public float dodgeCost = 20f;
    
    private PlayerStamina playerStamina;
    private bool isBlocking = false;
    
    void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
    }
    
    void Update()
    {
        // Handle blocking stamina drain
        if (isBlocking && playerStamina != null)
        {
            // Check if we have enough stamina for blocking
            if (playerStamina.CurrentStamina < blockCost * Time.deltaTime)
            {
                // Stop blocking if no stamina
                StopBlocking();
            }
            else
            {
                // Use stamina for blocking
                playerStamina.UseStamina(blockCost * Time.deltaTime);
            }
        }
    }
    
    // Light Attack
    public bool CanPerformLightAttack()
    {
        return playerStamina != null && playerStamina.HasStaminaForAttack && playerStamina.CurrentStamina >= lightAttackCost;
    }
    
    public bool PerformLightAttack()
    {
        if (CanPerformLightAttack())
        {
            playerStamina.UseStamina(lightAttackCost);
            return true;
        }
        return false;
    }
    
    // Heavy Attack
    public bool CanPerformHeavyAttack()
    {
        return playerStamina != null && playerStamina.CurrentStamina >= heavyAttackCost;
    }
    
    public bool PerformHeavyAttack()
    {
        if (CanPerformHeavyAttack())
        {
            playerStamina.UseStamina(heavyAttackCost);
            return true;
        }
        return false;
    }
    
    // Block
    public bool CanStartBlocking()
    {
        return playerStamina != null && playerStamina.CurrentStamina >= blockCost * Time.deltaTime;
    }
    
    public void StartBlocking()
    {
        if (CanStartBlocking())
        {
            isBlocking = true;
            Debug.Log("Started blocking - stamina will drain continuously");
        }
    }
    
    public void StopBlocking()
    {
        isBlocking = false;
        Debug.Log("Stopped blocking");
    }
    
    // Dodge
    public bool CanPerformDodge()
    {
        return playerStamina != null && playerStamina.CurrentStamina >= dodgeCost;
    }
    
    public bool PerformDodge()
    {
        if (CanPerformDodge())
        {
            playerStamina.UseStamina(dodgeCost);
            return true;
        }
        return false;
    }
    
    // Custom attack with specific cost
    public bool PerformCustomAttack(float staminaCost)
    {
        if (playerStamina != null && playerStamina.CurrentStamina >= staminaCost)
        {
            playerStamina.UseStamina(staminaCost);
            return true;
        }
        return false;
    }
    
    // Get current stamina status
    public bool HasStaminaForCombat()
    {
        return playerStamina != null && playerStamina.CurrentStamina > 0;
    }
    
    public float GetStaminaPercentage()
    {
        return playerStamina != null ? playerStamina.StaminaPercentage : 0f;
    }
} 