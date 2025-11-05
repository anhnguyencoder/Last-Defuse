# Hướng Dẫn Chi Tiết Setup Avatar Selection trong Unity

## Tổng Quan
Hướng dẫn này sẽ giúp bạn setup hệ thống chọn avatar trong Settings Panel với 5 avatar options, mỗi avatar có đầy đủ background, mask, overlay, image và dấu tích màu xanh khi được chọn.

## Bước 1: Chuẩn Bị Assets

Bạn cần có:
- 5 sprite avatar (từ Assets/Sprites hoặc nơi bạn lưu)
- Sprite dấu tích màu xanh (checkmark) - có thể tạo hoặc tìm trong Assets
- Các sprite UI: `UnitFrame_Avatar_Background`, `UnitFrame_Avatar_Mask`, `UnitFrame_Avatar_Overlay`

## Bước 2: Tạo UI Structure cho Avatar Selection

### 2.1. Tạo Container cho Avatar Selection

1. **Tìm Settings Panel** trong Hierarchy
2. **Right-click trên Settings Panel** → UI → Panel
3. **Đặt tên**: "Avatar Selection Container"
4. **Remove Image component** hoặc set Alpha = 0 (nếu không cần nền)
5. **Set Size và Position** phù hợp (ví dụ: Width = 800, Height = 200)

### 2.2. Tạo Label "Avatar:"

1. **Right-click trên "Avatar Selection Container"** → UI → Text - TextMeshPro
2. **Đặt tên**: "Avatar Label"
3. **Component TextMeshPro**:
   - Text: "Avatar:"
   - Font Size: 24-32 (tùy chỉnh)
   - Color: Trắng hoặc màu phù hợp
   - Alignment: Left
4. **Position**: Phía trên các avatar options

## Bước 3: Tạo Avatar Option 1

### 3.1. Tạo Button Container cho Avatar 1

1. **Right-click trên "Avatar Selection Container"** → UI → Button
2. **Đặt tên**: "Avatar Option 1"
3. **Remove Text component** (nếu có)
4. **Component Button**:
   - Transition: Color Tint hoặc Sprite Swap (tùy bạn)
   - Interactable: ✓
5. **Set Size**: Width = 120, Height = 120 (hoặc kích thước bạn muốn)
6. **Position**: X = 0 (hoặc tùy chỉnh)

### 3.2. Tạo Avatar Background

1. **Right-click trên "Avatar Option 1"** → UI → Image
2. **Đặt tên**: "Avatar Background"
3. **Component Image**:
   - Source Image: `UnitFrame_Avatar_Background`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 120, Height = 120
   - Position: (0, 0, 0)

### 3.3. Tạo Avatar Mask Container

1. **Right-click trên "Avatar Background"** → UI → Image
2. **Đặt tên**: "Avatar Mask Container"
3. **Component Image**:
   - Source Image: `UnitFrame_Avatar_Mask`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 120, Height = 120
   - Position: (0, 0, 0)
4. **Add Component** → Mask
   - Show Mask Graphic: ✗

### 3.4. Tạo Avatar Image

1. **Right-click trên "Avatar Mask Container"** → UI → Image
2. **Đặt tên**: "Avatar Image"
3. **Component Image**:
   - Source Image: (chọn sprite avatar 1 của bạn)
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 130, Height = 130 (lớn hơn một chút để fill)
   - Position: (0, 0, 0)

### 3.5. Tạo Avatar Overlay

1. **Right-click trên "Avatar Background"** → UI → Image
2. **Đặt tên**: "Avatar Overlay"
3. **Component Image**:
   - Source Image: `UnitFrame_Avatar_Overlay`
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 120, Height = 120
   - Position: (0, 0, 0)
4. **Set Order in Hierarchy**: Sau "Avatar Mask Container"

### 3.6. Tạo Checkmark (Dấu Tích)

1. **Right-click trên "Avatar Background"** → UI → Image
2. **Đặt tên**: "Checkmark"
3. **Component Image**:
   - Source Image: (Sprite dấu tích màu xanh)
   - Image Type: Simple
   - Preserve Aspect: ✓
   - Set Size: Width = 40, Height = 40 (hoặc kích thước phù hợp)
   - Position: Top Right (ví dụ: X = 40, Y = 40)
   - Color: Màu xanh (#00FF00 hoặc màu bạn muốn)
4. **Set Order in Hierarchy**: Sau "Avatar Overlay" (để hiển thị trên cùng)
5. **Set Active**: ✗ (Ẩn mặc định)

## Bước 4: Tạo Avatar Options 2-5

Lặp lại **Bước 3** cho 4 avatar còn lại:

1. **Avatar Option 2**: Tương tự như Avatar Option 1
2. **Avatar Option 3**: Tương tự như Avatar Option 1
3. **Avatar Option 4**: Tương tự như Avatar Option 1
4. **Avatar Option 5**: Tương tự như Avatar Option 1

**Lưu ý**: 
- Đặt tên: "Avatar Option 2", "Avatar Option 3", v.v.
- Gán sprite avatar khác nhau cho mỗi Avatar Image
- Đặt Position khác nhau để sắp xếp ngang (ví dụ: X = 0, 140, 280, 420, 560)

## Bước 5: Gán References vào SettingsPanel Script

### 5.1. Chọn GameObject có SettingsPanel Script

1. **Tìm GameObject** có script `SettingsPanel` trong Hierarchy
2. **Chọn GameObject** đó

### 5.2. Gán Avatar Label

1. **Trong Inspector**, tìm section **"Avatar Selection Settings"**
2. **Avatar Label**: Kéo GameObject "Avatar Label" vào

### 5.3. Gán Avatar Options List

1. **Avatar Options**: 
   - Click vào dropdown để mở rộng
   - **Size**: Nhập **5** (số lượng avatar)
   - Nhấn Enter

2. **Với mỗi Element (0-4)** - Chỉ cần gán 3 thứ:

   **Element 0 (Avatar Option 1)**:
   - **Avatar Button**: Kéo "Avatar Option 1" (Button) vào
   - **Avatar Sprite**: Click vào circle và chọn sprite avatar 1 từ Assets
   - **Checkmark**: Kéo "Checkmark" (con của Avatar Background) vào
   
   **Lưu ý**: Script sẽ tự động tìm Avatar Image trong children của button, không cần gán thủ công!

   **Element 1 (Avatar Option 2)**:
   - **Avatar Button**: Kéo "Avatar Option 2" (Button) vào
   - **Avatar Sprite**: Chọn sprite avatar 2 từ Assets
   - **Checkmark**: Kéo "Checkmark" vào

   **Element 2 (Avatar Option 3)**:
   - **Avatar Button**: Kéo "Avatar Option 3" (Button) vào
   - **Avatar Sprite**: Chọn sprite avatar 3 từ Assets
   - **Checkmark**: Kéo "Checkmark" vào

   **Element 3 (Avatar Option 4)**:
   - **Avatar Button**: Kéo "Avatar Option 4" (Button) vào
   - **Avatar Sprite**: Chọn sprite avatar 4 từ Assets
   - **Checkmark**: Kéo "Checkmark" vào

   **Element 4 (Avatar Option 5)**:
   - **Avatar Button**: Kéo "Avatar Option 5" (Button) vào
   - **Avatar Sprite**: Chọn sprite avatar 5 từ Assets
   - **Checkmark**: Kéo "Checkmark" vào

## Bước 6: Tạo Sprite Checkmark (Nếu chưa có)

### 6.1. Tạo Sprite Checkmark đơn giản

1. **Tạo Image mới**: Right-click trong Hierarchy → UI → Image
2. **Đặt tên**: "Checkmark Sprite"
3. **Component Image**:
   - Tạo sprite đơn giản hoặc import từ Assets
   - Hoặc dùng TextMeshPro với ký tự "✓" (Unicode: U+2713)

### 6.2. Hoặc Import Checkmark từ Assets

1. **Import sprite checkmark** vào Project (Assets/Sprites)
2. **Import Settings**:
   - Texture Type: Sprite (2D and UI)
   - Apply

## Bước 7: Sắp Xếp Layout

### 7.1. Sắp Xếp Avatar Options Ngang

1. **Chọn "Avatar Selection Container"**
2. **Sử dụng Layout Group**:
   - Add Component → Horizontal Layout Group
   - Spacing: 20 (khoảng cách giữa các avatar)
   - Child Alignment: Middle Center
   - Child Control Size: Width, Height
   - Child Force Expand: Width, Height

### 7.2. Hoặc Sắp Xếp Thủ Công

1. **Avatar Option 1**: Position X = 0
2. **Avatar Option 2**: Position X = 140
3. **Avatar Option 3**: Position X = 280
4. **Avatar Option 4**: Position X = 420
5. **Avatar Option 5**: Position X = 560

## Bước 8: Kiểm Tra và Test

### 8.1. Kiểm Tra References

1. **Chọn GameObject có SettingsPanel**
2. **Kiểm tra trong Inspector**:
   - ✅ Avatar Label đã được gán
   - ✅ Avatar Options có Size = 5
   - ✅ Mỗi Element có đầy đủ:
     - Avatar Button (đã gán)
     - Avatar Sprite (đã chọn sprite)
     - Checkmark (đã gán)
   
   **Lưu ý**: Avatar Image, Background, Mask, Overlay không cần gán - script sẽ tự động tìm!

### 8.2. Test trong Play Mode

1. **Play Game**
2. **Mở Settings Panel**
3. **Kiểm tra**:
   - Avatar options hiển thị đúng
   - Click vào avatar → Checkmark hiện lên
   - Click avatar khác → Checkmark chuyển sang avatar mới
   - Avatar trên PlayerUnitFrame cập nhật ngay

### 8.3. Test Lưu/Load

1. **Chọn avatar**
2. **Đóng game**
3. **Mở lại game**
4. **Kiểm tra**: Avatar đã chọn vẫn được giữ nguyên

## Bước 9: Tùy Chỉnh (Optional)

### 9.1. Thêm Hiệu Ứng Hover

1. **Chọn Avatar Button**
2. **Component Button**:
   - Transition: Color Tint
   - Highlighted Color: Màu sáng hơn
   - Pressed Color: Màu tối hơn

### 9.2. Thêm Animation

1. **Tạo Animation cho Checkmark**:
   - Animator Controller
   - Scale animation khi xuất hiện

### 9.3. Thêm Border cho Avatar Selected

1. **Thêm Image component** vào Avatar Background
2. **Hiển thị khi avatar được chọn**

## Troubleshooting

### Vấn đề: Checkmark không hiện

**Giải pháp**:
- Kiểm tra Checkmark Image có được gán vào AvatarOption.checkmark chưa
- Kiểm tra Order in Hierarchy (phải ở trên cùng)
- Kiểm tra Color Alpha = 1

### Vấn đề: Avatar không cập nhật trên PlayerUnitFrame

**Giải pháp**:
- Kiểm tra PlayerUnitFrame đã được gán vào UIController chưa
- Kiểm tra Avatar Sprite đã được gán trong Inspector chưa
- Kiểm tra Console có lỗi không

### Vấn đề: Avatar Image không hiển thị

**Giải pháp**:
- Kiểm tra Mask component đã được add vào Avatar Mask Container chưa
- Kiểm tra Avatar Image có nằm trong Avatar Mask Container không
- Kiểm tra Sprite đã được gán vào Avatar Image chưa

### Vấn đề: Button không click được

**Giải pháp**:
- Kiểm tra Button có Interactable = true không
- Kiểm tra Button có bị che bởi UI element khác không
- Kiểm tra Raycast Target = true

## Cấu Trúc Hierarchy Mẫu

```
Settings Panel
├── Avatar Selection Container
│   ├── Avatar Label
│   ├── Avatar Option 1 (Button)
│   │   └── Avatar Background (Image)
│   │       ├── Avatar Mask Container (Image + Mask)
│   │       │   └── Avatar Image (Image)
│   │       ├── Avatar Overlay (Image)
│   │       └── Checkmark (Image)
│   ├── Avatar Option 2 (Button)
│   │   └── ... (tương tự)
│   ├── Avatar Option 3 (Button)
│   ├── Avatar Option 4 (Button)
│   └── Avatar Option 5 (Button)
```

## Hoàn Thành!

Bây giờ bạn đã có hệ thống chọn avatar hoàn chỉnh. Người chơi có thể:
- Xem 5 avatar options trong Settings
- Click để chọn avatar
- Thấy dấu tích màu xanh trên avatar đã chọn
- Avatar tự động cập nhật trên PlayerUnitFrame
- Avatar được lưu và load khi khởi động lại game

Chúc bạn thành công! 🎮
