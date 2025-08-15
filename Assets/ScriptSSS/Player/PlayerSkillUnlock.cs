using UnityEngine;
using UnityEngine.Events;
using ScriptSSS.Quests;

public class PlayerSkillUnlock : MonoBehaviour
{
    [Header("Skill Unlock Settings")]
    [SerializeField] private KeyCode skillKey = KeyCode.Q;
    [SerializeField] private bool skillUnlocked = false;
    [SerializeField] private string requiredBossId = "boss";
    
    [Header("Skill VFX")]
    [SerializeField] private GameObject skillVFX;
    [SerializeField] private Transform skillSpawnPoint;
    [SerializeField] private float vfxDuration = 3f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip skillSound;
    [SerializeField] private AudioClip unlockSound;
    
    [Header("UI Feedback")]
    [SerializeField] private bool showSkillText = true;
    [SerializeField] private string skillMessage = "Skill Activated!";
    [SerializeField] private string unlockMessage = "Skill Unlocked!";
    [SerializeField] private string lockedMessage = "Skill Locked - Defeat Boss First!";
    
    [Header("Events")]
    public UnityEvent OnSkillUnlocked;
    public UnityEvent OnSkillUsed;
    public UnityEvent OnSkillLocked;
    
    private MainQuestManager questManager;
    private bool isUsingSkill = false;
    
    void Start()
    {
        // Tìm MainQuestManager
        questManager = MainQuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("PlayerSkillUnlock: Không tìm thấy MainQuestManager!");
        }
        
        // Tìm AudioSource nếu không được gán
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // Nếu không có skillSpawnPoint, sử dụng vị trí player
        if (skillSpawnPoint == null)
        {
            skillSpawnPoint = transform;
        }
        
        // Kiểm tra xem skill đã được unlock chưa
        CheckSkillUnlockStatus();
        
        // Subscribe to quest events
        if (questManager != null)
        {
            // Lắng nghe sự kiện khi quest stage thay đổi
            StartCoroutine(MonitorQuestProgress());
        }
    }
    
    void Update()
    {
        // Kiểm tra input phím Q
        if (Input.GetKeyDown(skillKey))
        {
            TryUseSkill();
        }
    }
    
    /// <summary>
    /// Coroutine để monitor quest progress
    /// </summary>
    private System.Collections.IEnumerator MonitorQuestProgress()
    {
        MainQuestStage lastStage = questManager.CurrentStage;
        
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Check mỗi 0.1 giây
            
            if (questManager.CurrentStage != lastStage)
            {
                lastStage = questManager.CurrentStage;
                CheckSkillUnlockStatus();
            }
        }
    }
    
    /// <summary>
    /// Kiểm tra trạng thái unlock của skill
    /// </summary>
    private void CheckSkillUnlockStatus()
    {
        if (questManager == null) return;
        
        bool wasUnlocked = skillUnlocked;
        
        // Skill được unlock khi đã hoàn thành quest KillBoss
        skillUnlocked = questManager.CurrentStage == MainQuestStage.ReturnToCastleNPC || 
                       questManager.CurrentStage == MainQuestStage.KillMainBoss || 
                       questManager.CurrentStage == MainQuestStage.Completed;
        
        // Nếu skill vừa được unlock
        if (!wasUnlocked && skillUnlocked)
        {
            OnSkillUnlocked?.Invoke();
            ShowUnlockMessage();
            PlayUnlockSound();
            Debug.Log($"🎉 Skill đã được unlock! Quest stage: {questManager.CurrentStage}");
        }
    }
    
    /// <summary>
    /// Thử sử dụng skill
    /// </summary>
    public void TryUseSkill()
    {
        if (isUsingSkill) return;
        
        if (!skillUnlocked)
        {
            // Skill chưa được unlock
            OnSkillLocked?.Invoke();
            ShowLockedMessage();
            Debug.LogWarning("🔒 Skill chưa được unlock! Hãy tiêu diệt boss trước.");
            return;
        }
        
        // Skill đã được unlock, sử dụng
        StartCoroutine(UseSkill());
    }
    
    /// <summary>
    /// Coroutine để sử dụng skill
    /// </summary>
    private System.Collections.IEnumerator UseSkill()
    {
        isUsingSkill = true;
        
        // Trigger event
        OnSkillUsed?.Invoke();
        
        // Spawn VFX skill
        if (skillVFX != null)
        {
            Vector3 spawnPos = skillSpawnPoint.position;
            GameObject vfx = Instantiate(skillVFX, spawnPos, Quaternion.identity);
            
            // Đảm bảo VFX hướng lên trên
            vfx.transform.rotation = Quaternion.identity;
            
            // Tự động destroy VFX sau một thời gian
            Destroy(vfx, vfxDuration);
            
            Debug.Log("✨ Đã spawn VFX skill!");
        }
        
        // Play sound skill
        if (audioSource != null && skillSound != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
        
        // Hiển thị text feedback
        if (showSkillText)
        {
            ShowSkillMessage();
        }
        
        // Chờ một chút để VFX bắt đầu
        yield return new WaitForSeconds(0.1f);
        
        // Thực hiện logic skill ở đây
        PerformSkillAction();
        
        // Chờ VFX hoàn thành
        yield return new WaitForSeconds(vfxDuration - 0.1f);
        
        isUsingSkill = false;
        
        Debug.Log("🎯 Skill đã được sử dụng thành công!");
    }
    
    /// <summary>
    /// Thực hiện hành động của skill
    /// </summary>
    private void PerformSkillAction()
    {
        // TODO: Thêm logic skill cụ thể ở đây
        // Ví dụ: Tăng damage, spawn projectiles, buff player, etc.
        
        Debug.Log("⚡ Thực hiện skill effect...");
        
        // Có thể thêm các effect như:
        // - Tăng damage tạm thời
        // - Spawn projectiles
        // - Buff player stats
        // - Area damage
        // - Teleport
        // - etc.
    }
    
    /// <summary>
    /// Hiển thị message khi sử dụng skill
    /// </summary>
    private void ShowSkillMessage()
    {
        if (showSkillText)
        {
            Debug.Log($"<color=blue>{skillMessage}</color>");
        }
    }
    
    /// <summary>
    /// Hiển thị message khi unlock skill
    /// </summary>
    private void ShowUnlockMessage()
    {
        if (showSkillText)
        {
            Debug.Log($"<color=green>{unlockMessage}</color>");
        }
    }
    
    /// <summary>
    /// Hiển thị message khi skill bị locked
    /// </summary>
    private void ShowLockedMessage()
    {
        if (showSkillText)
        {
            Debug.Log($"<color=red>{lockedMessage}</color>");
        }
    }
    
    /// <summary>
    /// Play sound khi unlock skill
    /// </summary>
    private void PlayUnlockSound()
    {
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }
    }
    
    /// <summary>
    /// Force unlock skill (dùng để test)
    /// </summary>
    public void ForceUnlockSkill()
    {
        skillUnlocked = true;
        OnSkillUnlocked?.Invoke();
        ShowUnlockMessage();
        PlayUnlockSound();
        Debug.Log("🔓 Skill đã được force unlock!");
    }
    
    /// <summary>
    /// Lock skill lại (dùng để test)
    /// </summary>
    public void LockSkill()
    {
        skillUnlocked = false;
        OnSkillLocked?.Invoke();
        Debug.Log("🔒 Skill đã bị lock!");
    }
    
    /// <summary>
    /// Kiểm tra xem skill có được unlock không
    /// </summary>
    /// <returns>True nếu skill đã được unlock</returns>
    public bool IsSkillUnlocked()
    {
        return skillUnlocked;
    }
    
    /// <summary>
    /// Lấy trạng thái quest hiện tại
    /// </summary>
    /// <returns>Quest stage hiện tại</returns>
    public MainQuestStage GetCurrentQuestStage()
    {
        return questManager != null ? questManager.CurrentStage : MainQuestStage.LeaveDungeon;
    }
    
    /// <summary>
    /// Lấy thông tin về boss cần tiêu diệt
    /// </summary>
    /// <returns>Thông tin boss</returns>
    public string GetBossInfo()
    {
        return $"Boss ID: {requiredBossId} | Quest Stage: {GetCurrentQuestStage()} | Skill Unlocked: {skillUnlocked}";
    }
    
    // Context menu để test từ Inspector
    [ContextMenu("Test Use Skill")]
    public void TestUseSkill()
    {
        TryUseSkill();
    }
    
    [ContextMenu("Force Unlock Skill")]
    public void TestForceUnlock()
    {
        ForceUnlockSkill();
    }
    
    [ContextMenu("Lock Skill")]
    public void TestLockSkill()
    {
        LockSkill();
    }
    
    [ContextMenu("Show Skill Info")]
    public void ShowSkillInfo()
    {
        Debug.Log(GetBossInfo());
    }
}
