# Panel Toggle Script - Hướng dẫn sử dụng

## Mô tả
Script này cho phép panel UI di chuyển sang trái khi nhấn phím V và trở về vị trí ban đầu khi nhấn V lần nữa.

## Cách sử dụng

### 1. Gán script vào GameObject
- Tạo một GameObject mới trong scene
- Gán script `PanelToggle.cs` hoặc `SimplePanelToggle.cs` vào GameObject đó

### 2. Thiết lập Panel
- Trong Inspector, gán `RectTransform` của panel UI vào trường `Panel`
- Panel phải có component `RectTransform` (thường là UI Image, Panel, hoặc Canvas)

### 3. Tùy chỉnh thông số
- **Slide Distance**: Khoảng cách di chuyển sang trái (mặc định: 300)
- **Slide Duration**: Thời gian di chuyển (mặc định: 0.3s) - chỉ có trong PanelToggle.cs
- **Slide Speed**: Tốc độ di chuyển (mặc định: 5) - chỉ có trong SimplePanelToggle.cs

### 4. Chạy game
- Nhấn phím **V** để toggle panel
- Panel sẽ di chuyển mượt mà sang trái/trở về vị trí ban đầu

## Hai phiên bản script

### PanelToggle.cs (Nâng cao)
- Animation mượt mà với EaseInOut
- Nhiều tùy chọn tùy chỉnh
- Có thể gọi từ script khác
- Xử lý trạng thái tốt hơn

### SimplePanelToggle.cs (Đơn giản)
- Code ngắn gọn, dễ hiểu
- Sử dụng MoveTowards đơn giản
- Phù hợp cho người mới bắt đầu

## Lưu ý quan trọng
1. **Panel phải có RectTransform**: Đảm bảo panel UI có component RectTransform
2. **Canvas Scaler**: Nếu sử dụng Canvas Scaler, có thể cần điều chỉnh khoảng cách di chuyển
3. **Input System**: Script sử dụng Input.GetKeyDown cũ, nếu dùng Input System mới cần thay đổi

## Ví dụ sử dụng
```csharp
// Gọi từ script khác
PanelToggle panelToggle = FindObjectOfType<PanelToggle>();
panelToggle.ShowPanel();  // Hiển thị panel
panelToggle.HidePanel();  // Ẩn panel
panelToggle.TogglePanel(); // Toggle panel
```

## Xử lý lỗi thường gặp
- **Panel không di chuyển**: Kiểm tra xem đã gán RectTransform vào script chưa
- **Di chuyển quá xa**: Giảm giá trị Slide Distance
- **Di chuyển quá chậm**: Tăng Slide Speed hoặc giảm Slide Duration
