# GoldDisplay Script - Hiển thị Gold độc lập

## Mô tả
Script `GoldDisplay` là một component độc lập để hiển thị số gold từ `CurrencyWallet` của player. Không cần phụ thuộc vào `PlayerUI`, có thể gán vào bất kỳ GameObject nào.

## Cách sử dụng

### 1. Thiết lập cơ bản
- Tạo một GameObject mới trong scene (ví dụ: "GoldUI")
- Gán script `GoldDisplay` vào GameObject đó
- Tạo TextMeshProUGUI component cho gold và gán vào field `goldText`
- Tùy chọn: Tạo Image component cho icon gold và gán vào field `goldIcon`

### 2. Thiết lập trong Inspector
```csharp
[Header("Gold UI")]
public TextMeshProUGUI goldText;        // Text hiển thị số gold
public Image goldIcon;                   // Icon gold (tùy chọn)
public bool showIcon = true;             // Bật/tắt hiển thị icon

[Header("Display Settings")]
public string prefix = "";               // Text trước số gold (ví dụ: "Gold: ")
public string suffix = "";               // Text sau số gold (ví dụ: " coins")
public bool useThousandSeparator = true; // Sử dụng dấu phẩy ngăn cách hàng nghìn
```

## Tính năng

### 1. Tự động cập nhật
- ✅ Kết nối tự động với `CurrencyWallet` của player
- ✅ Hiển thị gold ban đầu khi Start
- ✅ Cập nhật real-time khi gold thay đổi
- ✅ Sử dụng event system để tối ưu performance

### 2. Format hiển thị
- **Prefix/Suffix**: Thêm text trước và sau số gold
- **Thousand Separator**: Tự động thêm dấu phẩy ngăn cách hàng nghìn
- **Icon Support**: Hỗ trợ hiển thị icon gold

### 3. Hiệu ứng và Animation
- **Flash Effect**: Text flash màu vàng khi gold thay đổi
- **Customizable**: Dễ dàng tùy chỉnh hiệu ứng

## Ví dụ thiết lập

### Ví dụ 1: Hiển thị đơn giản
```
prefix: ""
suffix: ""
useThousandSeparator: true
Kết quả: "1,000"
```

### Ví dụ 2: Hiển thị với label
```
prefix: "Gold: "
suffix: ""
useThousandSeparator: true
Kết quả: "Gold: 1,000"
```

### Ví dụ 3: Hiển thị với đơn vị
```
prefix: ""
suffix: " coins"
useThousandSeparator: true
Kết quả: "1,000 coins"
```

### Ví dụ 4: Hiển thị không có dấu phẩy
```
prefix: "💰 "
suffix: ""
useThousandSeparator: false
Kết quả: "💰 1000"
```

## API Methods

### Public Methods
```csharp
// Cập nhật hiển thị
public void RefreshDisplay()

// Thiết lập prefix
public void SetPrefix(string newPrefix)

// Thiết lập suffix  
public void SetSuffix(string newPrefix)

// Bật/tắt hiển thị icon
public void SetIconVisibility(bool show)

// Bật/tắt dấu phẩy ngăn cách
public void SetThousandSeparator(bool useSeparator)
```

### Context Menu (Inspector)
- **Refresh Gold Display**: Cập nhật hiển thị
- **Test Flash Effect**: Test hiệu ứng flash

## Cách hoạt động

### 1. Khởi tạo
```csharp
void Start()
{
    // Tìm CurrencyWallet của player
    playerWallet = FindObjectOfType<ScriptSSS.InventorySystem.CurrencyWallet>();
    
    if (playerWallet != null)
    {
        // Subscribe vào event OnGoldChanged
        playerWallet.OnGoldChanged.AddListener(OnGoldChanged);
        
        // Hiển thị gold ban đầu
        UpdateGoldDisplay();
    }
}
```

### 2. Event Handling
```csharp
public void OnGoldChanged(int newGoldAmount)
{
    UpdateGoldDisplay();
    
    // Thêm hiệu ứng flash
    if (goldText != null)
    {
        StartCoroutine(FlashGoldText());
    }
}
```

### 3. Cập nhật hiển thị
```csharp
private void UpdateGoldDisplay()
{
    if (goldText == null || playerWallet == null) return;
    
    int currentGold = playerWallet.Gold;
    string formattedGold;
    
    if (useThousandSeparator)
    {
        formattedGold = currentGold.ToString("N0");
    }
    else
    {
        formattedGold = currentGold.ToString();
    }
    
    goldText.text = $"{prefix}{formattedGold}{suffix}";
}
```

## Tùy chỉnh nâng cao

### 1. Thay đổi hiệu ứng flash
```csharp
private System.Collections.IEnumerator FlashGoldText()
{
    if (goldText == null) yield break;
    
    Color originalColor = goldText.color;
    Color flashColor = Color.yellow; // Có thể thay đổi màu
    
    // Flash 3 lần với thời gian tùy chỉnh
    for (int i = 0; i < 3; i++)
    {
        goldText.color = flashColor;
        yield return new WaitForSeconds(0.1f); // Thời gian flash
        goldText.color = originalColor;
        yield return new WaitForSeconds(0.1f); // Thời gian nghỉ
    }
}
```

### 2. Thêm animation scale
```csharp
public void OnGoldChanged(int newGoldAmount)
{
    UpdateGoldDisplay();
    
    // Thêm hiệu ứng scale
    if (goldText != null)
    {
        StartCoroutine(ScaleGoldText());
    }
}

private System.Collections.IEnumerator ScaleGoldText()
{
    Vector3 originalScale = goldText.transform.localScale;
    Vector3 targetScale = originalScale * 1.2f;
    
    // Scale up
    float duration = 0.1f;
    float elapsed = 0f;
    
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float progress = elapsed / duration;
        goldText.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
        yield return null;
    }
    
    // Scale down
    elapsed = 0f;
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float progress = elapsed / duration;
        goldText.transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
        yield return null;
    }
    
    goldText.transform.localScale = originalScale;
}
```

## Troubleshooting

### Gold không hiển thị
1. **Kiểm tra CurrencyWallet**: Đảm bảo Player có component `CurrencyWallet`
2. **Kiểm tra goldText**: Đảm bảo đã gán TextMeshProUGUI vào field `goldText`
3. **Kiểm tra Console**: Xem có error message gì không

### Gold không cập nhật
1. **Kiểm tra Event**: Đảm bảo `OnGoldChanged` event đã được subscribe
2. **Kiểm tra CurrencyWallet**: Đảm bảo `AddGold()` hoặc `TrySpendGold()` được gọi
3. **Kiểm tra UpdateGoldDisplay**: Method này có được gọi không

### Performance Issues
1. **Event System**: Script sử dụng event system, không polling
2. **Memory Management**: Tự động unsubscribe events trong OnDestroy
3. **UI Updates**: Chỉ cập nhật khi cần thiết

## So sánh với PlayerUI

### PlayerUI (Trước đây)
- ❌ Phụ thuộc vào PlayerUI script
- ❌ Chỉ hiển thị được ở một nơi
- ❌ Khó tùy chỉnh riêng lẻ

### GoldDisplay (Hiện tại)
- ✅ **Độc lập hoàn toàn**
- ✅ **Có thể gán vào nhiều GameObject**
- ✅ **Dễ tùy chỉnh và mở rộng**
- ✅ **Không ảnh hưởng đến PlayerUI**

## Kết luận

`GoldDisplay` script cung cấp:
- 🎯 **Độc lập hoàn toàn** với PlayerUI
- 🔄 **Tự động cập nhật** khi gold thay đổi
- 🎨 **Dễ tùy chỉnh** format và hiệu ứng
- 📱 **Linh hoạt** - có thể gán vào bất kỳ đâu
- ⚡ **Performance tốt** với event system

Bây giờ bạn có thể hiển thị gold ở bất kỳ đâu trong game mà không cần phụ thuộc vào PlayerUI! 🪙✨
