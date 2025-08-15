# Gold UI System - Hướng dẫn sử dụng

## Mô tả
Hệ thống hiển thị số gold từ CurrencyWallet của player lên UI text. Gold sẽ tự động cập nhật khi có thay đổi.

## Cách thiết lập

### 1. Thiết lập CurrencyWallet
- Đảm bảo Player GameObject có component `CurrencyWallet`
- Component này sẽ quản lý số gold của player

### 2. Thiết lập PlayerUI
- Trong `PlayerUI` script, thêm các field sau:
  - **Gold Text**: TextMeshProUGUI component để hiển thị số gold
  - **Gold Icon**: Image component cho icon gold (tùy chọn)

### 3. Thiết lập UI Elements
- Tạo TextMeshProUGUI component cho gold
- Gán vào field `goldText` trong PlayerUI
- Có thể thêm icon gold nếu muốn

## Cách hoạt động

### 1. Tự động cập nhật
- Gold UI sẽ tự động cập nhật khi:
  - Game khởi động
  - Gold thay đổi (thêm/tiêu)
  - Player respawn

### 2. Event System
- `CurrencyWallet` sử dụng UnityEvent để thông báo khi gold thay đổi
- `PlayerUI` subscribe vào event này để cập nhật UI

### 3. Real-time Updates
- Không cần polling hoặc kiểm tra liên tục
- UI chỉ cập nhật khi có thay đổi thực sự

## Code Structure

### CurrencyWallet.cs
```csharp
public class CurrencyWallet : MonoBehaviour
{
    [SerializeField] private int gold;
    public UnityEvent<int> OnGoldChanged;
    
    public void AddGold(int amount)      // Thêm gold
    public bool TrySpendGold(int amount) // Tiêu gold
    public void SetGold(int newAmount)   // Set gold trực tiếp
}
```

### PlayerUI.cs
```csharp
public class PlayerUI : MonoBehaviour
{
    [Header("Gold UI")]
    public TextMeshProUGUI goldText;
    public Image goldIcon;
    
    private ScriptSSS.InventorySystem.CurrencyWallet playerWallet;
    
    private void UpdateGoldText()        // Cập nhật text
    public void OnGoldChanged(int gold)  // Event handler
}
```

## Cách sử dụng

### 1. Thêm Gold
```csharp
CurrencyWallet wallet = FindObjectOfType<CurrencyWallet>();
wallet.AddGold(100); // Thêm 100 gold
```

### 2. Tiêu Gold
```csharp
bool success = wallet.TrySpendGold(50);
if (success)
{
    Debug.Log("Đã tiêu 50 gold");
}
else
{
    Debug.Log("Không đủ gold");
}
```

### 3. Set Gold trực tiếp
```csharp
wallet.SetGold(1000); // Set về 1000 gold
```

## Testing

### GoldTester.cs
Script demo để test hệ thống gold:
- **G**: Thêm gold
- **H**: Tiêu gold  
- **R**: Reset gold về 0

### Context Menu
- Click chuột phải vào component trong Inspector
- Chọn "Add Gold", "Spend Gold", hoặc "Reset Gold"

## Troubleshooting

### Gold không hiển thị
1. Kiểm tra `CurrencyWallet` component đã được gán vào Player chưa
2. Kiểm tra `goldText` field đã được gán trong PlayerUI chưa
3. Kiểm tra Console để xem có error gì không

### Gold không cập nhật
1. Kiểm tra event `OnGoldChanged` đã được subscribe chưa
2. Kiểm tra method `UpdateGoldText()` có được gọi không
3. Kiểm tra `showText` có được set là `true` không

### Performance Issues
1. Đảm bảo chỉ subscribe/unsubscribe events một lần
2. Không gọi `UpdateGoldText()` liên tục trong Update
3. Sử dụng event system thay vì polling

## Tùy chỉnh

### Thay đổi format hiển thị
```csharp
private void UpdateGoldText()
{
    if (goldText != null && showText && playerWallet != null)
    {
        // Format với dấu phẩy
        goldText.text = $"{playerWallet.Gold:N0}";
        
        // Hoặc format với "Gold: "
        goldText.text = $"Gold: {playerWallet.Gold}";
        
        // Hoặc format với icon
        goldText.text = $"💰 {playerWallet.Gold}";
    }
}
```

### Thêm animation
```csharp
public void OnGoldChanged(int newGoldAmount)
{
    // Có thể thêm animation ở đây
    StartCoroutine(AnimateGoldChange(newGoldAmount));
    UpdateGoldText();
}
```

## Lưu ý quan trọng

### 1. Memory Management
- Luôn unsubscribe events trong OnDestroy
- Kiểm tra null trước khi sử dụng components

### 2. UI Updates
- Chỉ cập nhật UI khi cần thiết
- Sử dụng event system thay vì polling

### 3. Error Handling
- Kiểm tra null cho tất cả components
- Log warning khi không tìm thấy components cần thiết

## Kết luận

Hệ thống Gold UI này cung cấp:
- ✅ **Tự động cập nhật** khi gold thay đổi
- ✅ **Event-driven** architecture
- ✅ **Dễ bảo trì** và mở rộng
- ✅ **Performance tốt** với ít overhead
- ✅ **Testing tools** để debug và test
