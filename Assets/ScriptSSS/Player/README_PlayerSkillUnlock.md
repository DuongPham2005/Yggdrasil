# PlayerSkillUnlock System - Skill Unlock khi tiêu diệt Boss

## Mô tả
Hệ thống cho phép player sử dụng skill (phím Q) chỉ khi đã tiêu diệt được boss skeleton với ID "boss". Skill được unlock dựa trên tiến độ quest trong MainQuestManager.

## Cách hoạt động

### 1. **Quest Integration**
- **Skill Locked**: Khi quest stage < `KillBoss`
- **Skill Unlocked**: Khi quest stage >= `ReturnToCastleNPC` (sau khi tiêu diệt boss)
- **Auto Detection**: Tự động detect quest stage changes và unlock skill

### 2. **Boss Death Detection**
- Boss Skeleton phải có `questTargetId = "boss"` trong Enemy script
- Khi boss chết, `QuestEvents.RaiseEnemyKilled("boss")` được gọi
- MainQuestManager chuyển quest stage từ `KillBoss` → `ReturnToCastleNPC`
- PlayerSkillUnlock tự động unlock skill

### 3. **Skill Usage**
- **Phím Q**: Sử dụng skill (chỉ khi đã unlock)
- **VFX**: Spawn skill VFX với duration tùy chỉnh
- **Audio**: Play skill sound và unlock sound
- **Events**: UnityEvents để tích hợp với UI/Gameplay

## Cách thiết lập

### 1. **Thiết lập cơ bản**
- **Player GameObject**: Đảm bảo có component `MainQuestManager` (singleton)
- **Gán script**: Thêm `PlayerSkillUnlock` script vào Player GameObject
- **VFX Prefab**: Gán skill VFX vào field `skillVFX`

### 2. **Thiết lập Boss Skeleton**
```csharp
// Trong Enemy script của Boss Skeleton
[Header("Quest")] 
[SerializeField] string questTargetId = "boss"; // Quan trọng!
```

### 3. **Thiết lập trong Inspector**
```csharp
[Header("Skill Unlock Settings")]
public KeyCode skillKey = KeyCode.Q;           // Phím để sử dụng skill
public bool skillUnlocked = false;             // Trạng thái unlock (auto)
public string requiredBossId = "boss";         // ID boss cần tiêu diệt

[Header("Skill VFX")]
public GameObject skillVFX;                     // Prefab VFX skill
public Transform skillSpawnPoint;               // Vị trí spawn VFX
public float vfxDuration = 3f;                 // Thời gian VFX tồn tại

[Header("Audio")]
public AudioSource audioSource;                 // AudioSource component
public AudioClip skillSound;                    // Sound khi sử dụng skill
public AudioClip unlockSound;                   // Sound khi unlock skill

[Header("UI Feedback")]
public bool showSkillText = true;               // Hiển thị text feedback
public string skillMessage = "Skill Activated!"; // Nội dung text
public string unlockMessage = "Skill Unlocked!"; // Nội dung unlock
public string lockedMessage = "Skill Locked - Defeat Boss First!"; // Nội dung locked

[Header("Events")]
public UnityEvent OnSkillUnlocked;              // Event khi skill unlock
public UnityEvent OnSkillUsed;                  // Event khi sử dụng skill
public UnityEvent OnSkillLocked;                // Event khi skill bị locked
```

## Quest Flow để Unlock Skill

### 📋 **Quest Progression**
```
1. LeaveDungeon → 2. TalkToVillageNPC → 3. KillSkeletons → 
4. ReturnToVillageNPC → 5. GoToCastle → 6. TalkToCastleNPC → 
7. KillBoss → 8. ReturnToCastleNPC → 9. KillMainBoss → 10. Completed
```

### 🎯 **Skill Unlock Point**
- **Skill Locked**: Stages 1-7 (KillBoss)
- **Skill Unlocked**: Stages 8-10 (ReturnToCastleNPC, KillMainBoss, Completed)

### 🔍 **Boss Death Detection**
```csharp
// Trong Enemy script (Boss Skeleton)
void Die()
{
    // Raise quest event before destroy
    if (!string.IsNullOrEmpty(questTargetId))
    {
        QuestEvents.RaiseEnemyKilled(questTargetId); // "boss"
    }
    // ... rest of death logic
}
```

## Tính năng

### ✅ **Core Features**
- **Quest Integration**: Tích hợp với MainQuestManager
- **Auto Unlock**: Tự động unlock khi quest hoàn thành
- **Input Detection**: Nhận diện phím Q
- **VFX System**: Spawn và quản lý skill VFX
- **Audio Support**: Sound effects cho skill và unlock

### ✅ **Advanced Features**
- **Real-time Monitoring**: Monitor quest progress changes
- **Event System**: UnityEvents để tích hợp với UI/Gameplay
- **Status Checking**: Kiểm tra trạng thái skill
- **Force Unlock**: Force unlock để test (chỉ dùng test)

### ✅ **Safety Features**
- **Null Checks**: Kiểm tra components trước khi sử dụng
- **State Management**: Quản lý trạng thái skill usage
- **Error Handling**: Log errors khi thiếu components
- **Quest Validation**: Kiểm tra quest stage hợp lệ

## API Methods

### Public Methods
```csharp
// Thử sử dụng skill
public void TryUseSkill()

// Force unlock skill (dùng để test)
public void ForceUnlockSkill()

// Lock skill lại (dùng để test)
public void LockSkill()

// Kiểm tra xem skill có được unlock không
public bool IsSkillUnlocked()

// Lấy trạng thái quest hiện tại
public MainQuestStage GetCurrentQuestStage()

// Lấy thông tin về boss cần tiêu diệt
public string GetBossInfo()
```

### Events
```csharp
// Event khi skill được unlock
public UnityEvent OnSkillUnlocked

// Event khi skill được sử dụng
public UnityEvent OnSkillUsed

// Event khi skill bị locked
public UnityEvent OnSkillLocked
```

### Context Menu (Inspector)
- **Test Use Skill**: Test sử dụng skill
- **Force Heal 50**: Force unlock skill
- **Reset Cooldown**: Lock skill lại

## Testing

### 1. **SkillUnlockTester Script**
- **T**: Test sử dụng skill
- **Y**: Force unlock skill
- **U**: Lock skill
- **Context Menu**: Nhiều test methods

### 2. **Test Methods**
- **Test Quest Progression**: Kiểm tra tiến độ quest
- **Test Skill Unlock System**: Test toàn bộ hệ thống
- **Simulate Boss Death**: Giả lập boss chết
- **Reset Quest**: Reset quest về KillBoss stage

### 3. **Console Logs**
- Hiển thị trạng thái skill
- Hiển thị tiến độ quest
- Hiển thị thông báo unlock/locked

## Tùy chỉnh nâng cao

### 1. **Thay đổi skill behavior**
```csharp
private void PerformSkillAction()
{
    // Thêm logic skill cụ thể ở đây
    // Ví dụ: Tăng damage, spawn projectiles, buff player, etc.
    
    // Tăng damage tạm thời
    // Spawn projectiles
    // Buff player stats
    // Area damage
    // Teleport
    // etc.
}
```

### 2. **Thêm multiple VFX**
```csharp
[Header("Multiple VFX")]
[SerializeField] private GameObject[] skillVFXArray;
[SerializeField] private float vfxSpawnDelay = 0.1f;

private IEnumerator SpawnMultipleVFX()
{
    for (int i = 0; i < skillVFXArray.Length; i++)
    {
        if (skillVFXArray[i] != null)
        {
            Vector3 spawnPos = skillSpawnPoint.position + Vector3.up * i * 0.5f;
            GameObject vfx = Instantiate(skillVFXArray[i], spawnPos, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }
        yield return new WaitForSeconds(vfxSpawnDelay);
    }
}
```

### 3. **Tích hợp với UI**
```csharp
// Gán UI elements vào events
public class SkillUI : MonoBehaviour
{
    [SerializeField] private PlayerSkillUnlock skillUnlock;
    [SerializeField] private GameObject skillIcon;
    [SerializeField] private GameObject skillCooldown;
    
    void Start()
    {
        skillUnlock.OnSkillUnlocked.AddListener(OnSkillUnlocked);
        skillUnlock.OnSkillUsed.AddListener(OnSkillUsed);
        skillUnlock.OnSkillLocked.AddListener(OnSkillLocked);
    }
    
    private void OnSkillUnlocked()
    {
        skillIcon.SetActive(true);
        skillCooldown.SetActive(false);
    }
    
    private void OnSkillUsed()
    {
        // Show cooldown effect
        StartCoroutine(ShowCooldown());
    }
    
    private void OnSkillLocked()
    {
        skillIcon.SetActive(false);
        skillCooldown.SetActive(false);
    }
}
```

## Troubleshooting

### Skill không unlock
1. **Kiểm tra Boss ID**: Đảm bảo boss có `questTargetId = "boss"`
2. **Kiểm tra Quest Stage**: Đảm bảo quest đã đến `KillBoss` stage
3. **Kiểm tra MainQuestManager**: Đảm bảo singleton hoạt động
4. **Kiểm tra QuestEvents**: Đảm bảo event được raise khi boss chết

### Skill không hoạt động
1. **Kiểm tra skillUnlocked**: Đảm bảo skill đã được unlock
2. **Kiểm tra input**: Đảm bảo phím Q được nhận diện
3. **Kiểm tra VFX**: Đảm bảo skillVFX đã được gán
4. **Kiểm tra Audio**: Đảm bảo AudioSource hoạt động

### Quest không tiến triển
1. **Kiểm tra Enemy Death**: Đảm bảo `Die()` method được gọi
2. **Kiểm tra QuestEvents**: Đảm bảo `RaiseEnemyKilled` được gọi
3. **Kiểm tra MainQuestManager**: Đảm bảo event listener hoạt động

## Kết luận

Hệ thống PlayerSkillUnlock cung cấp:
- 🎯 **Quest Integration**: Tích hợp hoàn hảo với quest system
- 🔓 **Auto Unlock**: Tự động unlock khi tiêu diệt boss
- ⚡ **Skill System**: Hệ thống skill hoàn chỉnh với VFX/Audio
- 🧪 **Testing Tools**: Script test và context menu đầy đủ
- 🔧 **Easy Customization**: Dễ dàng tùy chỉnh và mở rộng

Bây giờ player phải tiêu diệt Boss Skeleton (ID: "boss") để unlock skill và sử dụng phím Q! 🎮✨👹
