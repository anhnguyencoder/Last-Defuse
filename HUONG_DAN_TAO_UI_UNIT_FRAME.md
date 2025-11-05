# Hướng Dẫn Tạo Thanh Máu và Avatar UI

## Tổng Quan
Hướng dẫn này sẽ giúp bạn tạo một Unit Frame UI với avatar, username, level và thanh máu theo phong cách game của bạn.

## Bước 1: Chuẩn Bị Assets

Bạn cần có các sprite sau trong thư mục Assets:
- `UnitFrame_Avatar_Background` - Nền cho avatar
- `UnitFrame_Avatar_Mask` - Mask để cắt avatar
- `UnitFrame_Avatar_Overlay` - Overlay trang trí cho avatar
- `UnitFrame_Health_Background` - Nền cho thanh máu
- `UnitFrame_Health_Fill_Tiled` - Fill cho thanh máu

## Bước 2: Tạo UI Canvas Structure

1. **Tạo Canvas mới** (nếu chưa có):
   - Right-click trong Hierarchy → UI → Canvas
   - Đặt tên: "Player Unit Frame Canvas"

2. **Tạo Unit Frame Container**:
   - Right-click trên Canvas → UI → Panel
   - Đặt tên: "Player Unit Frame"
   - Remove Image component (hoặc set Alpha = 0)
   - Set Anchor: Top Left
   - Position: X = 50, Y = -50 (tùy chỉnh theo vị trí bạn muốn)

## Bước 3: Tạo Avatar Section

### 3.1. Avatar Background
1. Right-click trên "Player Unit Frame" → UI → Image
2. Đặt tên: "Avatar Background"
3. Component Image:
   - Source Image: `UnitFrame_Avatar_Background`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 150, Height = 150 (tùy chỉnh)

### 3.2. Avatar Mask Container
1. Right-click trên "Avatar Background" → UI → Image
2. Đặt tên: "Avatar Mask Container"
3. Component Image:
   - Source Image: `UnitFrame_Avatar_Mask`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size giống Avatar Background
4. Add Component → Mask
   - Show Mask Graphic: ✗

### 3.3. Avatar Image
1. Right-click trên "Avatar Mask Container" → UI → Image
2. Đặt tên: "Avatar Image"
3. Component Image:
   - Source Image: (chọn avatar của player)
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: lớn hơn một chút so với container (để fill)

### 3.4. Avatar Overlay
1. Right-click trên "Avatar Background" → UI → Image
2. Đặt tên: "Avatar Overlay"
3. Component Image:
   - Source Image: `UnitFrame_Avatar_Overlay`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size giống Avatar Background
   - Set Order in Hierarchy: sau Avatar Mask Container

### 3.5. Level Badge
1. Right-click trên "Avatar Background" → UI → Panel
2. Đặt tên: "Level Badge"
3. Component Image:
   - Color: Đen hoặc xám đậm
   - Set Size: Width = 50, Height = 40
   - Position: Bottom Left của avatar (X = -75, Y = -75)
4. Right-click trên "Level Badge" → UI → Text - TextMeshPro
   - Đặt tên: "Level Text"
   - Text: "60"
   - Font Size: 24
   - Alignment: Center
   - Color: Cam hoặc vàng

## Bước 4: Tạo Player Info Section

### 4.1. Username Text
1. Right-click trên "Player Unit Frame" → UI → Text - TextMeshPro
2. Đặt tên: "Username Text"
3. Position: Bên phải avatar (X = 100, Y = 50)
4. Component TextMeshPro:
   - Text: "USERNAME"
   - Font Size: 32
   - Color: Cam (#FF8C00)
   - Alignment: Left

## Bước 5: Tạo Health Bar Section

### 5.1. Health Bar Background
1. Right-click trên "Player Unit Frame" → UI → Image
2. Đặt tên: "Health Bar Background"
3. Component Image:
   - Source Image: `UnitFrame_Health_Background`
   - Image Type: Sliced (hoặc Tiled)
   - Set Size: Width = 400, Height = 50
   - Position: Dưới Username (X = 100, Y = 0)

### 5.2. Health Bar Fill
1. Right-click trên "Health Bar Background" → UI → Image
2. Đặt tên: "Health Bar Fill"
3. Component Image:
   - Source Image: `UnitFrame_Health_Fill_Tiled`
   - Image Type: Filled (hoặc Tiled)
   - Fill Method: Horizontal
   - Fill Origin: Left
   - Fill Amount: 1
   - Set Size: Width = 400, Height = 50
   - Position: (0, 0, 0) - cùng vị trí với background

### 5.3. Health Text
1. Right-click trên "Health Bar Background" → UI → Text - TextMeshPro
2. Đặt tên: "Health Text"
3. Component TextMeshPro:
   - Text: "200/250"
   - Font Size: 28
   - Color: Trắng hoặc xám nhạt
   - Alignment: Center
   - Position: (0, 0, 0) - giữa thanh máu

## Bước 6: Gắn Script PlayerUnitFrame

1. **Thêm Script**:
   - Chọn "Player Unit Frame" trong Hierarchy
   - Add Component → Scripts → Player Unit Frame

2. **Gán References trong Inspector**:
   - **Avatar Image**: Kéo "Avatar Image" vào
   - **Avatar Background**: Kéo "Avatar Background" vào
   - **Avatar Mask**: Kéo "Avatar Mask Container" vào
   - **Avatar Overlay**: Kéo "Avatar Overlay" vào
   - **Health Bar Background**: Kéo "Health Bar Background" vào
   - **Health Bar Fill**: Kéo "Health Bar Fill" vào
   - **Health Text**: Kéo "Health Text" vào
   - **Username Text**: Kéo "Username Text" vào
   - **Level Text**: Kéo "Level Text" vào
   - **Player Username**: Nhập tên người chơi (mặc định: "USERNAME")
   - **Player Level**: Nhập level (mặc định: 60)

## Bước 7: Kết Nối với UIController

1. **Tìm UIController**:
   - Tìm GameObject có script UIController trong scene
   - Hoặc trong Canvas chính

2. **Gán Player Unit Frame**:
   - Chọn GameObject có UIController
   - Trong Inspector, tìm field "Player Unit Frame"
   - Kéo "Player Unit Frame" vào field này

## Bước 8: Tùy Chỉnh (Optional)

### 8.1. Thêm Avatar Sprite
- Nếu bạn có sprite avatar của player:
  - Import vào Project
  - Chọn "Avatar Image" trong Hierarchy
  - Gán sprite vào Source Image
  - Hoặc gọi `playerUnitFrame.SetAvatar(sprite)` trong code

### 8.2. Đổi Username/Level
- Trong code, bạn có thể gọi:
  ```csharp
  UIController.instance.playerUnitFrame.SetUsername("MyPlayer");
  UIController.instance.playerUnitFrame.SetLevel(99);
  ```

### 8.3. Tùy Chỉnh Màu Sắc
- Health Bar Fill có thể đổi màu dựa trên health %
- Username Text có thể đổi màu theo level
- Thêm các hiệu ứng như glow, shadow nếu cần

## Bước 9: Kiểm Tra

1. **Play Game**:
   - Chạy game và kiểm tra UI hiển thị đúng
   - Thử take damage để xem health bar giảm
   - Thử heal để xem health bar tăng

2. **Kiểm Tra Layout**:
   - Đảm bảo UI không bị che bởi các element khác
   - Kiểm tra ở các resolution khác nhau
   - Điều chỉnh Canvas Scaler nếu cần

## Lưu Ý

- **Canvas Scaler**: Nên dùng "Scale With Screen Size" cho responsive UI
- **Sort Order**: Đảm bảo Unit Frame có Sort Order cao để hiển thị trên cùng
- **Pivot & Anchors**: Sử dụng anchors để UI tự điều chỉnh theo màn hình
- **Performance**: Nếu có nhiều UI, cân nhắc dùng Object Pooling cho health bar

## Troubleshooting

- **Health Bar không cập nhật**: Kiểm tra PlayerUnitFrame đã được gán vào UIController chưa
- **Avatar không hiển thị**: Kiểm tra Mask component và Order in Hierarchy
- **Text không hiển thị**: Kiểm tra Canvas có Canvas Group với Alpha = 0 không
- **UI bị mờ**: Kiểm tra Canvas Scaler và Resolution settings

## Hoàn Thành!

Bây giờ bạn đã có một Unit Frame UI đẹp với avatar, username, level và thanh máu. Health bar sẽ tự động cập nhật khi player nhận damage hoặc heal!
