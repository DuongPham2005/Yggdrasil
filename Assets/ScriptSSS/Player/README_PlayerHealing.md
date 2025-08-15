# PlayerHealing System - Hồi máu với VFX

## Mô tả
Hệ thống cho phép player hồi máu khi nhấn phím H, kèm theo VFX healing và sound effects. Có cooldown để cân bằng game.

## Cách thiết lập

### 1. Thiết lập cơ bản
- **Player GameObject**: Đảm bảo có component `HealthSystem`
- **Gán script**: Thêm `PlayerHealing` script vào Player GameObject
- **VFX Prefab**: Gán healing VFX vào field `healVFX`

### 2. Thiết lập trong Inspector
```csharp
[Header("Healing Settings")]
public KeyCode healKey = KeyCode.H;        // Phím để hồi máu
public float healAmount = 25f;             // Lượng máu hồi
public float healCooldown = 3f;           // Cooldown (giây)
public bool canHeal = true;                // Bật/tắt khả năng hồi máu

[Header("VFX")]
public GameObject healVFX;                 // Prefab VFX healing
public Transform healSpawnPoint;           // Vị trí spawn VFX
public float vfxDuration = 2f;            // Thời gian VFX tồn tại

[Header("Audio")]
public AudioSource audioSource;            // AudioSource component
public AudioClip healSound;                // Sound khi hồi máu

[Header("UI Feedback")]
public bool showHealText = true;           // Hiển thị text feedback
public string healMessage = "Healed!";     // Nội dung text
```

## Cách hoạt động

### 1. Input System
- Player nhấn phím **H** để hồi máu
- Script kiểm tra cooldown và điều kiện hồi máu
- Nếu hợp lệ, bắt đầu quá trình healing

### 2. VFX System
- Spawn VFX healing tại vị trí `healSpawnPoint`
- VFX tự động destroy sau `vfxDuration` giây
- VFX hướng lên trên (Quaternion.identity)

### 3. Audio System
- Play sound healing nếu có `healSound`
- Sử dụng `PlayOneShot` để không bị gián đoạn

### 4. Health System
- Gọi `playerHealth.Heal(healAmount)` để hồi máu
- Tự động cập nhật UI thông qua HealthSystem

## Tính năng

### ✅ **Core Features**
- **Input Detection**: Nhận diện phím H
- **Cooldown System**: Hạn chế spam healing
- **VFX Integration**: Spawn và quản lý VFX
- **Audio Support**: Sound effects
- **Health Integration**: Tích hợp với HealthSystem

### ✅ **Advanced Features**
- **Force Heal**: Hồi máu từ bên ngoài
- **Cooldown Management**: Kiểm tra và reset cooldown
- **Status Checking**: Kiểm tra có thể hồi máu không
- **Customizable**: Dễ dàng tùy chỉnh thông số

### ✅ **Safety Features**
- **Null Checks**: Kiểm tra components trước khi sử dụng
- **State Management**: Quản lý trạng thái healing
- **Error Handling**: Log errors khi thiếu components

## API Methods

### Public Methods
```csharp
// Thử hồi máu (cần input)
public void TryHeal()

// Hồi máu từ bên ngoài (không cần input)
public void ForceHeal(float amount)

// Bật/tắt khả năng hồi máu
public void SetHealingEnabled(bool enabled)

// Thiết lập cooldown mới
public void SetHealCooldown(float newCooldown)

// Thiết lập lượng máu hồi mới
public void SetHealAmount(float newAmount)

// Kiểm tra có thể hồi máu không
public bool CanHeal()

// Lấy thời gian cooldown còn lại
public float GetCooldownRemaining()
```

### Context Menu (Inspector)
- **Test Heal**: Test hồi máu
- **Force Heal 50**: Hồi 50 máu
- **Reset Cooldown**: Reset cooldown về 0

## VFX Recommendations

### 1. **VFX có sẵn trong dự án**
- `vfx_Heal_01.prefab` - VFX healing cơ bản
- `vfx_Heal_02.prefab` - VFX healing nâng cao
- `Healing.prefab` - VFX healing từ Hovl Studio
- `Healing circle.prefab` - VFX healing circle

### 2. **VFX Requirements**
- **Duration**: Nên có duration phù hợp với `vfxDuration`
- **Scale**: VFX nên có scale phù hợp với player
- **Particle System**: Nên sử dụng Particle System để dễ quản lý

### 3. **VFX Placement**
- **healSpawnPoint**: Nên đặt ở vị trí trung tâm player
- **Offset**: Có thể thêm offset để VFX không bị che khuất

## Audio Setup

### 1. **AudioSource Component**
- Thêm `AudioSource` component vào Player
- Gán vào field `audioSource` trong PlayerHealing
- Hoặc script sẽ tự động tìm

### 2. **Heal Sound**
- Sử dụng audio clip phù hợp với theme game
- Format: .wav, .mp3, .ogg
- Duration: Nên ngắn (1-3 giây)

## Testing

### 1. **HealingTester Script**
- **J**: Gây damage để test healing
- **K**: Reset HP về max
- **H**: Test hồi máu (nếu có PlayerHealing)

### 2. **Context Menu Testing**
- Click chuột phải vào component
- Chọn các test methods tương ứng

### 3. **Console Logs**
- Hiển thị thông tin healing
- Hiển thị trạng thái cooldown
- Hiển thị thông báo lỗi

## Troubleshooting

### VFX không hiển thị
1. **Kiểm tra healVFX**: Đảm bảo đã gán prefab
2. **Kiểm tra healSpawnPoint**: Đảm bảo vị trí spawn hợp lệ
3. **Kiểm tra vfxDuration**: Đảm bảo VFX không bị destroy quá sớm

### Sound không phát
1. **Kiểm tra AudioSource**: Đảm bảo component đã được gán
2. **Kiểm tra healSound**: Đảm bảo audio clip đã được gán
3. **Kiểm tra volume**: Đảm bảo AudioSource không bị mute

### Healing không hoạt động
1. **Kiểm tra HealthSystem**: Đảm bảo Player có component này
2. **Kiểm tra cooldown**: Đảm bảo không còn trong cooldown
3. **Kiểm tra canHeal**: Đảm bảo healing được bật

### Performance Issues
1. **VFX Duration**: Không nên quá dài để tránh lag
2. **VFX Scale**: Không nên quá lớn
3. **Audio Clips**: Nên sử dụng format nén (.ogg)

## Tùy chỉnh nâng cao

### 1. **Thay đổi VFX behavior**
```csharp
private IEnumerator HealPlayer()
{
    // Spawn VFX với rotation tùy chỉnh
    if (healVFX != null)
    {
        Vector3 spawnPos = healSpawnPoint.position;
        GameObject vfx = Instantiate(healVFX, spawnPos, Quaternion.identity);
        
        // Thêm offset
        vfx.transform.position += Vector3.up * 1f;
        
        // Thay đổi scale
        vfx.transform.localScale *= 1.5f;
        
        Destroy(vfx, vfxDuration);
    }
    
    // ... rest of the code
}
```

### 2. **Thêm multiple VFX**
```csharp
[Header("Multiple VFX")]
[SerializeField] private GameObject[] healVFXArray;
[SerializeField] private float vfxSpawnDelay = 0.1f;

private IEnumerator SpawnMultipleVFX()
{
    for (int i = 0; i < healVFXArray.Length; i++)
    {
        if (healVFXArray[i] != null)
        {
            Vector3 spawnPos = healSpawnPoint.position + Vector3.up * i * 0.5f;
            GameObject vfx = Instantiate(healVFXArray[i], spawnPos, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }
        yield return new WaitForSeconds(vfxSpawnDelay);
    }
}
```

### 3. **Thêm healing particles**
```csharp
[Header("Particle Effects")]
[SerializeField] private ParticleSystem healParticles;

private void PlayHealParticles()
{
    if (healParticles != null)
    {
        healParticles.Play();
    }
}
```

## Kết luận

Hệ thống PlayerHealing cung cấp:
- 🎮 **Input System**: Nhận diện phím H để hồi máu
- ✨ **VFX Integration**: Spawn VFX healing đẹp mắt
- 🔊 **Audio Support**: Sound effects khi hồi máu
- ⏰ **Cooldown System**: Cân bằng game
- 🔧 **Easy Customization**: Dễ dàng tùy chỉnh
- 🧪 **Testing Tools**: Script test và context menu

Bây giờ player có thể hồi máu bằng cách nhấn phím H với VFX đẹp mắt! 💚✨
