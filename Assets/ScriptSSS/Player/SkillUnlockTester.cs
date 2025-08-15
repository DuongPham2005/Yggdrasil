using UnityEngine;
using ScriptSSS.Quests;

public class SkillUnlockTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private bool showSkillInfo = true;
    [SerializeField] private bool showQuestProgress = true;
    
    [Header("Test Controls")]
    [SerializeField] private KeyCode testSkillKey = KeyCode.T;
    [SerializeField] private KeyCode forceUnlockKey = KeyCode.Y;
    [SerializeField] private KeyCode lockSkillKey = KeyCode.U;
    
    private PlayerSkillUnlock playerSkill;
    private MainQuestManager questManager;
    
    void Start()
    {
        // Tìm components cần thiết
        playerSkill = FindObjectOfType<PlayerSkillUnlock>();
        questManager = MainQuestManager.Instance;
        
        if (playerSkill == null)
        {
            Debug.LogError("SkillUnlockTester: Không tìm thấy PlayerSkillUnlock!");
        }
        
        if (questManager == null)
        {
            Debug.LogError("SkillUnlockTester: Không tìm thấy MainQuestManager!");
        }
        
        // Hiển thị thông tin skill
        if (showSkillInfo)
        {
            ShowSkillInfo();
        }
    }
    
    void Update()
    {
        if (playerSkill == null) return;
        
        // Test sử dụng skill
        if (Input.GetKeyDown(testSkillKey))
        {
            Debug.Log("🧪 Test sử dụng skill...");
            playerSkill.TryUseSkill();
        }
        
        // Force unlock skill
        if (Input.GetKeyDown(forceUnlockKey))
        {
            Debug.Log("🔓 Force unlock skill...");
            playerSkill.ForceUnlockSkill();
        }
        
        // Lock skill
        if (Input.GetKeyDown(lockSkillKey))
        {
            Debug.Log("🔒 Lock skill...");
            playerSkill.LockSkill();
        }
        
        // Hiển thị thông tin skill mỗi giây
        if (showSkillInfo && Time.frameCount % 60 == 0)
        {
            ShowSkillStatus();
        }
        
        // Hiển thị quest progress mỗi 2 giây
        if (showQuestProgress && questManager != null && Time.frameCount % 120 == 0)
        {
            ShowQuestProgress();
        }
    }
    
    /// <summary>
    /// Hiển thị thông tin về hệ thống skill
    /// </summary>
    private void ShowSkillInfo()
    {
        if (playerSkill == null) return;
        
        Debug.Log("=== HỆ THỐNG SKILL UNLOCK ===");
        Debug.Log("Nhấn Q để sử dụng skill (nếu đã unlock)");
        Debug.Log("Nhấn T để test skill");
        Debug.Log("Nhấn Y để force unlock skill");
        Debug.Log("Nhấn U để lock skill");
        Debug.Log("===============================");
    }
    
    /// <summary>
    /// Hiển thị trạng thái skill
    /// </summary>
    private void ShowSkillStatus()
    {
        if (playerSkill == null) return;
        
        string status = $"Skill Status: {(playerSkill.IsSkillUnlocked() ? "🔓 UNLOCKED" : "🔒 LOCKED")}";
        
        if (playerSkill.IsSkillUnlocked())
        {
            status += " | ✅ Có thể sử dụng";
        }
        else
        {
            status += " | ❌ Cần tiêu diệt boss trước";
        }
        
        // Chỉ log khi có thay đổi để tránh spam
        Debug.Log(status);
    }
    
    /// <summary>
    /// Hiển thị tiến độ quest
    /// </summary>
    private void ShowQuestProgress()
    {
        if (questManager == null) return;
        
        string questInfo = $"Quest Progress: {questManager.CurrentStage}";
        
        switch (questManager.CurrentStage)
        {
            case MainQuestStage.LeaveDungeon:
                questInfo += " | 🚪 Rời khỏi dungeon";
                break;
            case MainQuestStage.TalkToVillageNPC:
                questInfo += " | 🏘️ Nói chuyện với NPC làng";
                break;
            case MainQuestStage.KillSkeletons:
                questInfo += $" | 💀 Tiêu diệt {questManager.SkeletonKilled}/{questManager.SkeletonRequired} skeleton";
                break;
            case MainQuestStage.ReturnToVillageNPC:
                questInfo += " | 🏘️ Quay lại NPC làng";
                break;
            case MainQuestStage.GoToCastle:
                questInfo += " | 🏰 Đi đến lâu đài";
                break;
            case MainQuestStage.TalkToCastleNPC:
                questInfo += " | 🏰 Nói chuyện với NPC lâu đài";
                break;
            case MainQuestStage.KillBoss:
                questInfo += " | 👹 Tiêu diệt Boss Skeleton (ID: boss)";
                break;
            case MainQuestStage.ReturnToCastleNPC:
                questInfo += " | 🏰 Quay lại NPC lâu đài (Skill sẽ được unlock!)";
                break;
            case MainQuestStage.KillMainBoss:
                questInfo += " | 🐉 Tiêu diệt Main Boss";
                break;
            case MainQuestStage.Completed:
                questInfo += " | 🎉 Hoàn thành tất cả quest!";
                break;
        }
        
        Debug.Log(questInfo);
    }
    
    /// <summary>
    /// Test quest progression
    /// </summary>
    [ContextMenu("Test Quest Progression")]
    public void TestQuestProgression()
    {
        if (questManager == null) return;
        
        Debug.Log($"Current Quest Stage: {questManager.CurrentStage}");
        Debug.Log($"Skeletons Killed: {questManager.SkeletonKilled}/{questManager.SkeletonRequired}");
        
        // Hiển thị hướng dẫn để unlock skill
        if (questManager.CurrentStage == MainQuestStage.KillBoss)
        {
            Debug.Log("🎯 Để unlock skill, hãy tiêu diệt Boss Skeleton với ID 'boss'");
            Debug.Log("📍 Boss thường ở trong dungeon hoặc boss arena");
        }
        else if (questManager.CurrentStage == MainQuestStage.ReturnToCastleNPC)
        {
            Debug.Log("🎉 Skill đã được unlock! Hãy nhấn Q để sử dụng");
        }
        else if (questManager.CurrentStage < MainQuestStage.KillBoss)
        {
            Debug.Log("⏳ Chưa đến quest tiêu diệt boss. Hãy hoàn thành các quest trước");
        }
    }
    
    /// <summary>
    /// Test skill unlock system
    /// </summary>
    [ContextMenu("Test Skill Unlock System")]
    public void TestSkillUnlockSystem()
    {
        if (playerSkill == null) return;
        
        Debug.Log("=== TEST SKILL UNLOCK SYSTEM ===");
        Debug.Log($"Skill Unlocked: {playerSkill.IsSkillUnlocked()}");
        Debug.Log($"Current Quest Stage: {playerSkill.GetCurrentQuestStage()}");
        Debug.Log($"Boss Info: {playerSkill.GetBossInfo()}");
        
        // Test các trường hợp
        if (!playerSkill.IsSkillUnlocked())
        {
            Debug.Log("🔒 Skill chưa unlock - Test sử dụng skill (sẽ bị từ chối)");
            playerSkill.TryUseSkill();
        }
        else
        {
            Debug.Log("🔓 Skill đã unlock - Test sử dụng skill");
            playerSkill.TryUseSkill();
        }
        
        Debug.Log("================================");
    }
    
    /// <summary>
    /// Simulate boss death
    /// </summary>
    [ContextMenu("Simulate Boss Death")]
    public void SimulateBossDeath()
    {
        if (questManager == null) return;
        
        Debug.Log("🎭 Simulating boss death...");
        
        // Gọi event enemy killed với boss ID
        QuestEvents.RaiseEnemyKilled("boss");
        
        Debug.Log("✅ Boss death event đã được trigger!");
        Debug.Log("🔄 Kiểm tra xem skill có được unlock không...");
        
        // Đợi một frame để event được xử lý
        StartCoroutine(CheckSkillAfterBossDeath());
    }
    
    private System.Collections.IEnumerator CheckSkillAfterBossDeath()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (playerSkill != null)
        {
            Debug.Log($"Skill Status sau khi boss chết: {(playerSkill.IsSkillUnlocked() ? "🔓 UNLOCKED" : "🔒 LOCKED")}");
            Debug.Log($"Quest Stage: {playerSkill.GetCurrentQuestStage()}");
        }
    }
    
    /// <summary>
    /// Reset quest về trạng thái ban đầu
    /// </summary>
    [ContextMenu("Reset Quest to KillBoss Stage")]
    public void ResetQuestToKillBoss()
    {
        if (questManager == null) return;
        
        Debug.Log("🔄 Resetting quest về KillBoss stage...");
        
        // Sử dụng reflection để set quest stage (chỉ dùng để test)
        var questType = typeof(MainQuestManager);
        var currentStageField = questType.GetField("currentStage", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (currentStageField != null)
        {
            currentStageField.SetValue(questManager, MainQuestStage.KillBoss);
            Debug.Log("✅ Quest đã được reset về KillBoss stage");
            Debug.Log("🎯 Bây giờ hãy tiêu diệt boss để unlock skill!");
        }
        else
        {
            Debug.LogWarning("⚠️ Không thể reset quest stage (reflection failed)");
        }
    }
    
    /// <summary>
    /// Show all available test methods
    /// </summary>
    [ContextMenu("Show All Test Methods")]
    public void ShowAllTestMethods()
    {
        Debug.Log("=== TẤT CẢ PHƯƠNG THỨC TEST ===");
        Debug.Log("🧪 Test Quest Progression");
        Debug.Log("🎯 Test Skill Unlock System");
        Debug.Log("🎭 Simulate Boss Death");
        Debug.Log("🔄 Reset Quest to KillBoss Stage");
        Debug.Log("📋 Show All Test Methods");
        Debug.Log("================================");
    }
}
